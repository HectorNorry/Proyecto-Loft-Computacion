using LoftComputacion.Application.DTOs;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;                  // ← Necesario para Stream
using Microsoft.AspNetCore.Http;

namespace LoftComputacion.Application
{
    public class OrdenDeServicioService
    {
        private readonly ApplicationDbContext _context;
        private readonly AIService _aiService;
        private readonly EmailService _emailService;
        private readonly BlobService _blobService;

        private const int ID_ESTADO_FINALIZADO_ESPERA_PAGO = 4;
        private const int ID_ESTADO_FINALIZADO_ENTREGADO = 5;

        public OrdenDeServicioService(
            ApplicationDbContext context,
            AIService aiService,
            BlobService blobService,
            EmailService emailService)
        {
            _context = context;
            _aiService = aiService;
            _emailService = emailService;
            _blobService = blobService;
        }

        // ================== CRUD BÁSICO ==================

        public async Task<IEnumerable<OrdenListaDto>> GetAllOrdenesAsync(string? filtro)
        {
            var query = _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(o =>
                    o.Id.ToString() == filtro ||
                    (o.Cliente != null &&
                     o.Cliente.NombreCompleto != null &&
                     o.Cliente.NombreCompleto.Contains(filtro)) ||
                    (o.Equipo != null &&
                     o.Equipo.Modelo != null &&
                     o.Equipo.Modelo.Contains(filtro)));
            }

            var resultado = await query
                .OrderByDescending(o => o.FechaIngreso)
                .Select(o => new OrdenListaDto
                {
                    Id = o.Id,
                    FechaIngreso = o.FechaIngreso,
                    FallaDeclaradaPorCliente = o.FallaDeclaradaPorCliente,
                    NombreCliente = o.Cliente != null ? o.Cliente.NombreCompleto : "N/A",
                    NombreEstado = o.Estado != null ? o.Estado.Nombre : "N/A",
                    ModeloEquipo = o.Equipo != null ? o.Equipo.Modelo : "N/A",
                    PrecioFinal = o.PrecioFinal
                })
                .ToListAsync();

            return resultado;
        }

        public async Task<OrdenDeServicio?> GetOrdenByIdAsync(int id)
        {
            return await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .Include(o => o.Fotos)
                .Include(o => o.Historial).ThenInclude(h => h.Usuario)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Método "viejo" que crea una orden recibiendo directamente la entidad.
        /// Lo dejo por compatibilidad con lo que ya tenías.
        /// </summary>
        public async Task<OrdenDeServicio> CreateOrdenAsync(OrdenDeServicio orden)
        {
            orden.EstadoId = 1; // Recibido
            orden.FechaIngreso = DateTime.UtcNow;

            _context.OrdenesDeServicio.Add(orden);
            await _context.SaveChangesAsync();

            return orden;
        }

        /// <summary>
        /// NUEVO: crear orden desde CreateOrdenDto (lo que usa Blazor).
        /// </summary>
        public async Task<int> CrearOrdenAsync(CreateOrdenDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
                throw new Exception("El cliente no existe.");

            // === CREAR EQUIPO ===
            var equipo = new Equipo
            {
                Tipo = dto.TipoEquipo,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                NumeroDeSerie = dto.NumeroSerie,
                // Si querés guardar una descripción extra, la ponemos en Componentes:
                Componentes = dto.DescripcionCompleta
            };

            // === CREAR ORDEN ===
            var orden = new OrdenDeServicio
            {
                ClienteId = dto.ClienteId,
                Cliente = cliente,
                Equipo = equipo,
                EquipoId = equipo.Id, // EF lo completa, pero lo dejamos por claridad

                FechaIngreso = DateTime.UtcNow,
                EstadoId = 1, // RECIBIDO

                FallaDeclaradaPorCliente = dto.FallaDeclaradaPorCliente,
                PrecioPresupuestado = dto.PrecioPresupuestado,
                MetodoDePagoId = dto.MetodoDePagoId
            };

            _context.Equipos.Add(equipo);
            _context.OrdenesDeServicio.Add(orden);
            await _context.SaveChangesAsync();

            return orden.Id;
        }

        public async Task<bool> UpdateOrdenAsync(int id, OrdenDeServicio ordenConNuevosDatos, int usuarioId)
        {
            var ordenExistente = await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordenExistente == null)
                return false;

            int estadoAnteriorId = ordenExistente.EstadoId;
            int estadoNuevoId = ordenConNuevosDatos.EstadoId;

            var estadoAnterior = await _context.Estados
                .FirstOrDefaultAsync(e => e.Id == estadoAnteriorId);

            var estadoNuevo = await _context.Estados
                .FirstOrDefaultAsync(e => e.Id == estadoNuevoId);

            string nombreEstadoAnterior = estadoAnterior?.Nombre ?? "(desconocido)";
            string nombreEstadoNuevo = estadoNuevo?.Nombre ?? "(desconocido)";

            ordenExistente.EstadoId = estadoNuevoId;
            ordenExistente.PrecioPresupuestado = ordenConNuevosDatos.PrecioPresupuestado;
            ordenExistente.PrecioFinal = ordenConNuevosDatos.PrecioFinal;
            ordenExistente.ResumenTecnico = ordenConNuevosDatos.ResumenTecnico;

            await _context.SaveChangesAsync();

            var historial = new HistorialOrden
            {
                OrdenDeServicioId = ordenExistente.Id,
                UsuarioId = usuarioId,
                FechaHora = DateTime.UtcNow,
                DescripcionDelCambio = $"Estado cambiado de {nombreEstadoAnterior} a {nombreEstadoNuevo}"
            };

            await _context.HistorialOrdenes.AddAsync(historial);
            await _context.SaveChangesAsync();

            if (estadoNuevoId == ID_ESTADO_FINALIZADO_ESPERA_PAGO &&
                estadoAnteriorId != ID_ESTADO_FINALIZADO_ESPERA_PAGO)
            {
                string resumenParaEmail;

                if (!string.IsNullOrWhiteSpace(ordenExistente.ResumenTecnico))
                    resumenParaEmail = await _aiService.GenerarResumenDesdeTecnicoAsync(
                        ordenExistente.ResumenTecnico);
                else
                    resumenParaEmail = await _aiService.GenerarResumenDesdeFallaAsync(
                        ordenExistente.FallaDeclaradaPorCliente);

                if (string.IsNullOrEmpty(resumenParaEmail))
                {
                    resumenParaEmail =
                        "¡Tu equipo está listo y funcionando correctamente! " +
                        "Ya puedes pasar a retirarlo.\n\nSaludos,\nEl equipo de LOFT COMPUTACIÓN";
                }

                if (ordenExistente.Cliente != null &&
                    !string.IsNullOrEmpty(ordenExistente.Cliente.Email))
                {
                    await _emailService.EnviarEmailNotificacion(
                        ordenExistente.Cliente.Email,
                        ordenExistente.Cliente.NombreCompleto,
                        resumenParaEmail,
                        ordenExistente.Id);
                }
            }

            return true;
        }

        public async Task<bool> DeleteOrdenAsync(int id)
        {
            var orden = await _context.OrdenesDeServicio.FindAsync(id);
            if (orden == null)
                return false;

            _context.OrdenesDeServicio.Remove(orden);
            await _context.SaveChangesAsync();
            return true;
        }

        // ================== DASHBOARD ==================

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var ordenesHoy = await _context.OrdenesDeServicio
                .CountAsync(o => o.FechaIngreso.Date == hoy);

            var enTaller = await _context.OrdenesDeServicio
                .CountAsync(o => o.Estado.Nombre != "Cancelado" &&
                                 o.Estado.Nombre != "Entregado");

            var finalizadas = await _context.OrdenesDeServicio
                .CountAsync(o => o.Estado.Nombre == "Finalizado, a espera de pago");

            var ingresos = await _context.OrdenesDeServicio
                .Where(o => o.PrecioFinal.HasValue && o.FechaIngreso >= inicioMes)
                .SumAsync(o => o.PrecioFinal ?? 0);

            var estados = await _context.OrdenesDeServicio
                .GroupBy(o => o.Estado.Nombre)
                .Select(g => new { Estado = g.Key, Cant = g.Count() })
                .ToDictionaryAsync(e => e.Estado, e => e.Cant);

            var colores = new Dictionary<string, string>
            {
                { "Recibido", "#2196F3" },
                { "Finalizado, a espera de pago", "#9C27B0" },
                { "Cancelado", "#E53935" },
                { "En Proceso", "#FB8C00" },
                { "Esperando Aprobación", "#EC407A" },
                { "Entregado", "#43A047" }
            };

            var hace7Dias = DateTime.Today.AddDays(-7);

            var alertas = new DashboardAlertas
            {
                EsperandoAprobacion = estados.ContainsKey("Esperando Aprobación")
                    ? estados["Esperando Aprobación"]
                    : 0,

                MasDe7Dias = await _context.OrdenesDeServicio
                    .CountAsync(o => o.FechaIngreso < hace7Dias &&
                                     o.Estado.Nombre != "Entregado" &&
                                     o.Estado.Nombre != "Cancelado"),

                ListasParaEntregar = finalizadas
            };

            var topClientes = await _context.OrdenesDeServicio
                .Where(o => o.FechaIngreso >= inicioMes)
                .GroupBy(o => o.Cliente.NombreCompleto)
                .Select(g => new TopClienteDto
                {
                    Nombre = g.Key,
                    TotalGenerado = g.Sum(o => o.PrecioFinal ?? 0),
                    CantidadOrdenes = g.Count()
                })
                .OrderByDescending(x => x.TotalGenerado)
                .Take(5)
                .ToListAsync();

            var ultimas = await _context.OrdenesDeServicio
                .OrderByDescending(o => o.FechaIngreso)
                .Take(10)
                .Select(o => new OrdenSimpleDto
                {
                    Id = o.Id,
                    Fecha = o.FechaIngreso,
                    ClienteNombre = o.Cliente.NombreCompleto,
                    Estado = o.Estado.Nombre,
                    Total = o.PrecioFinal
                })
                .ToListAsync();

            return new DashboardDto
            {
                OrdenesHoy = ordenesHoy,
                EnTaller = enTaller,
                Finalizadas = finalizadas,
                Ingresos = ingresos,
                Estados = estados,
                EstadoColores = colores,
                UltimasOrdenes = ultimas,
                Alertas = alertas,
                TopClientes = topClientes
            };
        }

        public async Task<IEnumerable<HistorialOrden>> GetHistorialByOrdenIdAsync(int ordenId)
        {
            return await _context.HistorialOrdenes
                .Where(h => h.OrdenDeServicioId == ordenId)
                .Include(h => h.Usuario)
                .OrderByDescending(h => h.FechaHora)
                .ToListAsync();
        }

        public async Task<OrdenDetalleDto?> GetOrdenDetalleAsync(int id)
        {
            var orden = await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .Include(o => o.Fotos)
                .Include(o => o.Historial).ThenInclude(h => h.Usuario)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
                return null;

            return new OrdenDetalleDto
            {
                Id = orden.Id,
                FechaIngreso = orden.FechaIngreso,
                ClienteNombre = orden.Cliente.NombreCompleto,
                EmailCliente = orden.Cliente.Email,
                Estado = orden.Estado.Nombre,
                PrecioPresupuestado = orden.PrecioPresupuestado,
                PrecioFinal = orden.PrecioFinal,

                EquipoModelo = orden.Equipo.Modelo,
                EquipoTipo = orden.Equipo.Tipo.ToString().Replace("_", " "),
                EquipoDescripcion = orden.Equipo.DescripcionCompleta,

                FallaDeclarada = orden.FallaDeclaradaPorCliente,
                ResumenTecnico = orden.ResumenTecnico,

                Fotos = orden.Fotos
    .Select(f => new FotoDto
    {
        Id = f.Id,
        Url = f.RutaArchivo
    })
    .ToList(),

                Historial = orden.Historial
                    .OrderByDescending(h => h.FechaHora)
                    .Select(h => new HistorialLineaDto
                    {
                        FechaHora = h.FechaHora,
                        Usuario = h.Usuario.NombreCompleto,
                        Descripcion = h.DescripcionDelCambio
                    })
                    .ToList()
            };
        }

        public async Task<Foto> SubirFotoAsync(IFormFile archivo, int ordenId)
        {
            var orden = await _context.OrdenesDeServicio
                .Include(o => o.Fotos)
                .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden == null)
                throw new Exception("La orden no existe.");

            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"orden-{ordenId}-{Guid.NewGuid()}{extension}";

            using var stream = archivo.OpenReadStream();

            var url = await _blobService.UploadFileAsync(stream, nombreArchivo, "fotos");

            // Insertar en SQL
            var foto = new Foto
            {
                RutaArchivo = url,
                OrdenDeServicioId = ordenId
            };

            _context.Fotos.Add(foto);
            await _context.SaveChangesAsync();

            return foto;
        }



    }
}
