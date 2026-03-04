using LoftComputacion.Shared.DTOs;
using LoftComputacion.Shared.Enums;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
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
        private const int ID_ESTADO_PAGADO = 7;
        private const int UsuarioSistemaId = 14;

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

        public async Task<IEnumerable<OrdenListaDto>> GetAllOrdenesAsync(string? filtro, DateTime? fechaDesde = null, DateTime? fechaHasta = null, string? estado = null, string? tipo = null)
        {
            var query = _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .AsQueryable();

            // 1. Buscador por cliente (Mantenemos el ToLower por seguridad)
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.ToLower().Trim();
                query = query.Where(o => o.Cliente != null && o.Cliente.NombreCompleto.ToLower().Contains(f));
            }

            // 2. EL ARREGLO PARA EL TIPO DE EQUIPO
            if (!string.IsNullOrWhiteSpace(tipo))
            {
                // PASO CLAVE: Convertimos el string "Impresora" al Enum real (ej: TipoEquipo.Impresora)
                // Reemplazá 'TipoEquipo' por el nombre exacto de tu Enum si es distinto
                if (Enum.TryParse<LoftComputacion.Shared.Enums.TipoDeEquipo>(tipo, out var tipoEnum))
                {
                    // Ahora comparamos Enum contra Enum (int contra int en SQL)
                    query = query.Where(o => o.Equipo != null && o.Equipo.Tipo == tipoEnum);
                }
            }

            // 3. Filtros de Fechas y Estado (Igual que antes)
            if (fechaDesde.HasValue) query = query.Where(o => o.FechaIngreso >= fechaDesde.Value.Date);
            if (fechaHasta.HasValue) query = query.Where(o => o.FechaIngreso <= fechaHasta.Value.Date.AddDays(1).AddTicks(-1));
            if (!string.IsNullOrWhiteSpace(estado)) query = query.Where(o => o.Estado.Nombre == estado);

            return await query
                .OrderByDescending(o => o.FechaIngreso)
                .Select(o => new OrdenListaDto
                {
                    Id = o.Id,
                    FechaIngreso = o.FechaIngreso,
                    NombreCliente = o.Cliente != null ? o.Cliente.NombreCompleto : "N/A",
                    NombreEstado = o.Estado != null ? o.Estado.Nombre : "N/A",
                    PrecioFinal = o.PrecioFinal
                })
                .ToListAsync();
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

        public async Task<OrdenDeServicio> CreateOrdenAsync(OrdenDeServicio orden)
        {
            orden.EstadoId = 1;
            orden.FechaIngreso = DateTime.UtcNow;

            _context.OrdenesDeServicio.Add(orden);
            await _context.SaveChangesAsync();

            return orden;
        }

        public async Task<int> CrearOrdenAsync(CreateOrdenDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
                throw new Exception("El cliente no existe.");

            var equipo = new Equipo
            {
                Tipo = dto.TipoEquipo,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                NumeroDeSerie = dto.NumeroSerie,
                Componentes = dto.DescripcionCompleta
            };

            var orden = new OrdenDeServicio
            {
                ClienteId = dto.ClienteId,
                Cliente = cliente,
                Equipo = equipo,
                EquipoId = equipo.Id,
                FechaIngreso = DateTime.UtcNow,
                EstadoId = 1,
                FallaDeclaradaPorCliente = dto.FallaDeclaradaPorCliente,
                PrecioPresupuestado = dto.PrecioPresupuestado,
                MetodoDePagoId = dto.MetodoDePagoId
            };

            _context.Equipos.Add(equipo);
            _context.OrdenesDeServicio.Add(orden);
            await _context.SaveChangesAsync();

            return orden.Id;
        }

        public async Task<bool> UpdateOrdenAsync(int id, OrdenDeServicio ordenActualizada, int usuarioId, string? usuarioAutorizador = null)
        {
            var ordenDb = await _context.OrdenesDeServicio
                .Include(o => o.Estado)
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordenDb == null) return false;

            int estadoAnteriorId = ordenDb.EstadoId;
            var nombreEstadoAnterior = ordenDb.Estado?.Nombre ?? "Desconocido";

            ordenDb.PrecioPresupuestado = ordenActualizada.PrecioPresupuestado;
            ordenDb.PrecioFinal = ordenActualizada.PrecioFinal;
            ordenDb.ResumenTecnico = ordenActualizada.ResumenTecnico;

            if (estadoAnteriorId != ordenActualizada.EstadoId)
            {
                var estadoNuevoObj = await _context.Estados.FindAsync(ordenActualizada.EstadoId);
                string nombreEstadoNuevo = estadoNuevoObj?.Nombre ?? "Desconocido";

                int idUsuarioResponsable = usuarioId > 0 ? usuarioId : UsuarioSistemaId;
                string nombreResponsable = "Usuario del sistema";

                if (idUsuarioResponsable > 0)
                {
                    var usuarioDb = await _context.Usuarios.FindAsync(idUsuarioResponsable);
                    if (usuarioDb != null)
                    {
                        nombreResponsable = !string.IsNullOrWhiteSpace(usuarioDb.NombreCompleto)
                                            ? usuarioDb.NombreCompleto
                                            : (usuarioDb.Email ?? "Usuario desconocido");
                    }
                }

                var historial = new HistorialOrden
                {
                    OrdenDeServicioId = ordenDb.Id,
                    FechaHora = DateTime.UtcNow,
                    UsuarioId = idUsuarioResponsable,
                    DescripcionDelCambio = $"Estado cambiado de '{nombreEstadoAnterior}' a '{nombreEstadoNuevo}' (Responsable: {nombreResponsable})"
                };

                _context.HistorialOrdenes.Add(historial);

                if (ordenActualizada.EstadoId == ID_ESTADO_FINALIZADO_ESPERA_PAGO)
                {
                    try
                    {
                        string resumenTecnico = ordenActualizada.ResumenTecnico ?? "El equipo ha sido reparado exitosamente.";
                        string? resumenIA = await _aiService.GenerarResumenDesdeTecnicoAsync(resumenTecnico);

                        if (resumenIA != null && ordenDb.Cliente != null && !string.IsNullOrEmpty(ordenDb.Cliente.Email))
                        {
                            await _emailService.EnviarEmailNotificacion(
                                ordenDb.Cliente.Email,
                                ordenDb.Cliente.NombreCompleto,
                                resumenIA,
                                ordenDb.Id
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error enviando email: {ex.Message}");
                    }
                }

                ordenDb.EstadoId = ordenActualizada.EstadoId;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteOrdenAsync(int id)
        {
            var orden = await _context.OrdenesDeServicio.FindAsync(id);
            if (orden == null) return false;

            _context.OrdenesDeServicio.Remove(orden);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var ordenesHoy = await _context.OrdenesDeServicio.CountAsync(o => o.FechaIngreso.Date == hoy);

            var enTaller = await _context.OrdenesDeServicio.CountAsync(o => o.Estado.Nombre != "Cancelado" && o.Estado.Nombre != "Entregado");

            var finalizadas = await _context.OrdenesDeServicio.CountAsync(o => o.Estado.Nombre == "Finalizado, a espera de pago");

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
                EsperandoAprobacion = estados.ContainsKey("Esperando Aprobación") ? estados["Esperando Aprobación"] : 0,
                MasDe7Dias = await _context.OrdenesDeServicio.CountAsync(o => o.FechaIngreso < hace7Dias && o.Estado.Nombre != "Entregado" && o.Estado.Nombre != "Cancelado"),
                ListasParaEntregar = finalizadas
            };

            var topClientes = await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Estado)
                .Where(o => o.FechaIngreso >= inicioMes)
                .GroupBy(o => o.Cliente)
                .Select(g => new TopClienteDto
                {
                    Nombre = g.Key.NombreCompleto,
                    CantidadOrdenes = g.Count(),
                    TotalGenerado = g.Where(o => o.Estado.Nombre == "Pagado" || o.Estado.Nombre == "Entregado").Sum(o => o.PrecioFinal ?? 0)
                })
                .Where(x => x.TotalGenerado > 0)
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

            if (orden == null) return null;

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
                Fotos = orden.Fotos.Select(f => new FotoDto { Id = f.Id, Url = f.RutaArchivo }).ToList(),
                Historial = orden.Historial.OrderByDescending(h => h.FechaHora)
                    .Select(h => new HistorialLineaDto
                    {
                        FechaHora = h.FechaHora,
                        Usuario = h.Usuario.NombreCompleto,
                        Descripcion = h.DescripcionDelCambio
                    }).ToList()
            };
        }

        public async Task<Foto> SubirFotoAsync(IFormFile archivo, int ordenId)
        {
            var orden = await _context.OrdenesDeServicio.Include(o => o.Fotos).FirstOrDefaultAsync(o => o.Id == ordenId);
            if (orden == null) throw new Exception("La orden no existe.");

            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"orden-{ordenId}-{Guid.NewGuid()}{extension}";

            using var stream = archivo.OpenReadStream();
            var url = await _blobService.UploadFileAsync(stream, nombreArchivo, "fotos");

            var foto = new Foto
            {
                RutaArchivo = url,
                OrdenDeServicioId = ordenId
            };

            _context.Fotos.Add(foto);
            await _context.SaveChangesAsync();

            return foto;
        }

        public async Task ActualizarEstadoPorPagoAsync(int ordenId, string notaHistorial)
        {
            var orden = await _context.OrdenesDeServicio.Include(o => o.Estado).FirstOrDefaultAsync(o => o.Id == ordenId);
            if (orden == null) return;

            string estadoAnterior = orden.Estado.Nombre;
            orden.EstadoId = ID_ESTADO_PAGADO;
            orden.FechaPago = DateTime.UtcNow;

            var historial = new HistorialOrden
            {
                OrdenDeServicioId = orden.Id,
                FechaHora = DateTime.UtcNow,
                UsuarioId = UsuarioSistemaId,
                DescripcionDelCambio = $"Estado cambiado de '{estadoAnterior}' a 'Pagado' por Mercado Pago. {notaHistorial}"
            };

            _context.HistorialOrdenes.Add(historial);
            await _context.SaveChangesAsync();
        }

        public async Task<MetricasDto> GetMetricasOperativasAsync()
        {
            var metricas = new MetricasDto();
            var fechaInicio = DateTime.UtcNow.AddDays(-30);

            var porTipo = await _context.OrdenesDeServicio
                .Include(o => o.Equipo)
                .Where(o => o.FechaIngreso >= fechaInicio)
                .GroupBy(o => o.Equipo.Tipo)
                .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            metricas.OrdenesPorTipo = porTipo.Select(x => new DatoGrafico
            {
                Etiqueta = x.Tipo.ToString(),
                Valor = x.Cantidad
            }).ToList();

            var ordenesCerradas = await _context.OrdenesDeServicio
                .Where(o => o.FechaIngreso >= fechaInicio &&
                            (o.EstadoId == ID_ESTADO_FINALIZADO_ENTREGADO || o.EstadoId == ID_ESTADO_PAGADO || o.EstadoId == 6))
                .ToListAsync();

            metricas.CantidadFinalizadas = ordenesCerradas.Count(o => o.EstadoId != 6);
            metricas.CantidadCanceladas = ordenesCerradas.Count(o => o.EstadoId == 6);
            metricas.TotalOrdenesMes = await _context.OrdenesDeServicio.CountAsync(o => o.FechaIngreso >= fechaInicio);

            if ((metricas.CantidadFinalizadas + metricas.CantidadCanceladas) > 0)
            {
                metricas.TasaRebote = Math.Round((double)metricas.CantidadCanceladas / (metricas.CantidadFinalizadas + metricas.CantidadCanceladas) * 100, 1);
            }

            var historialEventos = await _context.HistorialOrdenes
                .Include(h => h.Usuario)
                .Where(h => h.FechaHora >= fechaInicio &&
                            (h.DescripcionDelCambio.Contains("Entregado") || h.DescripcionDelCambio.Contains("Pagado")))
                .ToListAsync();

            metricas.RendimientoTecnicos = historialEventos
                .GroupBy(h => h.Usuario?.NombreCompleto ?? "Sistema")
                .Select(g => new DatoGrafico { Etiqueta = g.Key, Valor = g.Count() })
                .OrderByDescending(x => x.Valor)
                .ToList();

            return metricas;
        }
    }
}