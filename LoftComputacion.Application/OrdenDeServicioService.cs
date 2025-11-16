using LoftComputacion.Application.DTOs;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure; // Asumo que aquí está tu DbContext
using Microsoft.EntityFrameworkCore;


namespace LoftComputacion.Application
{
    public class OrdenDeServicioService
    {
        private readonly ApplicationDbContext _context; // O tu Repositorio
        private readonly AIService _aiService;

        // --- CORRECCIÓN ---
        // Cambiado de IEmailService a EmailService (basado en tu confirmación)
        private readonly EmailService _emailService;

        // IDs de Estado (¡AJUSTA ESTOS VALORES!)
        private const int ID_ESTADO_FINALIZADO_ESPERA_PAGO = 4;
        private const int ID_ESTADO_FINALIZADO_ENTREGADO = 5; // (Ejemplo, si tienes otro)


        // --- ¡CORRECCIÓN EN EL CONSTRUCTOR! ---
        public OrdenDeServicioService(
            ApplicationDbContext context,
            AIService aiService,
            EmailService emailService) // Cambiado a EmailService
        {
            _context = context;
            _aiService = aiService;
            _emailService = emailService;
        }

        /// <summary>
        /// Método interno para actualizar el estado de una orden y
        /// generar un registro de historial (ej: para Webhooks).
        /// </summary>
        /// <param name="ordenId">ID de la orden a cambiar</param>
        /// <param name="nuevoEstadoId">El ID del nuevo estado (ej: 7 para "Pagado")</param>
        /// <param name="notaDeHistorial">El texto que se guardará en el historial</param>
        public async Task ActualizarEstadoOrdenAsync(int ordenId, int nuevoEstadoId, string notaDeHistorial)
        {
            // 1. Buscamos la orden
            var ordenExistente = await _context.OrdenesDeServicio.FindAsync(ordenId);

            if (ordenExistente == null)
            {
                // Si la orden no existe, no podemos hacer nada.
                // (En un sistema real, aquí se "loguearía" este error)
                return;
            }

            // 2. Verificamos si el estado ya está aplicado
            //    (Esto evita duplicados si MP manda el webhook varias veces)
            if (ordenExistente.EstadoId == nuevoEstadoId)
            {
                return; // Ya está en este estado, no hacemos nada.
            }

            // 3. Actualizamos el estado de la orden
            ordenExistente.EstadoId = nuevoEstadoId;

            // 4. Creamos la nueva entrada de historial
            var historialEntry = new HistorialOrden
            {
                OrdenDeServicioId = ordenId,
                DescripcionDelCambio = notaDeHistorial,
                FechaHora = DateTime.UtcNow,

                // ¡IMPORTANTE! El webhook se ejecuta sin un usuario logueado.
                // Debemos asignar un ID de usuario "Sistema" o "Admin".
                // Usaremos '1' asumiendo que es el ID de tu usuario Admin principal.
                // TODO: En el futuro, idealmente crear un usuario "Sistema" con ID fijo.
                UsuarioId = 1
            };

            // 5. Agregamos el historial al contexto
            await _context.HistorialOrdenes.AddAsync(historialEntry);

            // 6. Guardamos ambos cambios (la orden y el historial) en la DB
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<OrdenListaDto>> GetAllOrdenesAsync(string? filtro)
        {
            var query = _context.OrdenesDeServicio
                                .Include(o => o.Cliente)
                                .Include(o => o.Equipo)
                                .Include(o => o.Estado)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                // Aplicamos el filtro seguro que ya tenías
                query = query.Where(o =>
                    (o.Id.ToString() == filtro) ||
                    (o.Cliente != null && o.Cliente.NombreCompleto != null && o.Cliente.NombreCompleto.Contains(filtro)) ||
                    (o.Equipo != null && o.Equipo.Modelo != null && o.Equipo.Modelo.Contains(filtro))
                );
            }

            // Proyectamos (aplanamos) al DTO simple
            var resultado = await query
                .OrderByDescending(o => o.FechaIngreso)
                .Select(o => new OrdenListaDto
                {
                    Id = o.Id,
                    FechaIngreso = o.FechaIngreso,
                    FallaDeclaradaPorCliente = o.FallaDeclaradaPorCliente,
                    NombreCliente = (o.Cliente != null) ? o.Cliente.NombreCompleto : "N/A",
                    NombreEstado = (o.Estado != null) ? o.Estado.Nombre : "N/A", // Asegúrate que tu entidad Estado tenga 'Nombre'
                    ModeloEquipo = (o.Equipo != null) ? o.Equipo.Modelo : "N/A", // Asegúrate que tu entidad Equipo tenga 'Modelo'
                    PrecioFinal = o.PrecioFinal
                })
                .ToListAsync();

            return resultado;
        }

        // --- AGREGAR ESTE MÉTODO NUEVO ---
        public async Task<IEnumerable<HistorialOrden>> GetHistorialByOrdenIdAsync(int ordenId)
        {
            return await _context.HistorialOrdenes
                .Where(h => h.OrdenDeServicioId == ordenId)
                .Include(h => h.Usuario) // ¡Importante para mostrar el nombre del usuario!
                .OrderByDescending(h => h.FechaHora)
                .ToListAsync();
        }
        public async Task<OrdenDeServicio?> GetOrdenByIdAsync(int id)
        {
            // ... (Tu código de GetById)
            return await _context.OrdenesDeServicio
                               .Include(o => o.Cliente)
                               .Include(o => o.Equipo)
                               .Include(o => o.Estado)
                               .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrdenDeServicio> CreateOrdenAsync(OrdenDeServicio orden)
        {
            // Estado inicial "Recibido"
            orden.EstadoId = 1;

            // ✅ ASIGNAR FECHA DE INGRESO
            orden.FechaIngreso = DateTime.UtcNow;  // 🔥 ESTA ES LA LÍNEA QUE FALTABA

            _context.OrdenesDeServicio.Add(orden);
            await _context.SaveChangesAsync();

            return orden;
        }


        // --- ¡AQUÍ ESTÁ TODA LA LÓGICA NUEVA! ---
        public async Task<bool> UpdateOrdenAsync(int id, OrdenDeServicio ordenConNuevosDatos, int usuarioId)
        {
            // 1. Cargamos la orden existente con Cliente (para email)
            var ordenExistente = await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordenExistente == null)
                return false;

            // 2. Guardamos los IDs del estado anterior y nuevo
            int estadoAnteriorId = ordenExistente.EstadoId;
            int estadoNuevoId = ordenConNuevosDatos.EstadoId;

            // 3. CARGAMOS LOS NOMBRES DE LOS ESTADOS
            var estadoAnterior = await _context.Estados
                .FirstOrDefaultAsync(e => e.Id == estadoAnteriorId);

            var estadoNuevo = await _context.Estados
                .FirstOrDefaultAsync(e => e.Id == estadoNuevoId);

            string nombreEstadoAnterior = estadoAnterior?.Nombre ?? "(desconocido)";
            string nombreEstadoNuevo = estadoNuevo?.Nombre ?? "(desconocido)";

            // 4. Actualizamos datos
            ordenExistente.EstadoId = estadoNuevoId;
            ordenExistente.PrecioPresupuestado = ordenConNuevosDatos.PrecioPresupuestado;
            ordenExistente.PrecioFinal = ordenConNuevosDatos.PrecioFinal;
            ordenExistente.ResumenTecnico = ordenConNuevosDatos.ResumenTecnico;

            // 5. Guardamos la orden
            await _context.SaveChangesAsync();

            // 6. AGREGAMOS HISTORIAL (CORRECCIÓN FINAL)
            var historial = new HistorialOrden
            {
                OrdenDeServicioId = ordenExistente.Id,
                UsuarioId = usuarioId,
                FechaHora = DateTime.UtcNow,
                DescripcionDelCambio =
                    $"Estado cambiado de {nombreEstadoAnterior} a {nombreEstadoNuevo}"
            };

            await _context.HistorialOrdenes.AddAsync(historial);
            await _context.SaveChangesAsync();

            // 7. Lógica de IA y email (NO SE TOCA)
            if (estadoNuevoId == ID_ESTADO_FINALIZADO_ESPERA_PAGO &&
                estadoAnteriorId != ID_ESTADO_FINALIZADO_ESPERA_PAGO)
            {
                string resumenParaEmail = "";

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
                        ordenExistente.Id
                    );
                }
            }

            return true;
        }



        public async Task<bool> DeleteOrdenAsync(int id)
        {
            // ... (Tu código de Delete)
            var orden = await _context.OrdenesDeServicio.FindAsync(id);
            if (orden == null)
            {
                return false;
            }

            _context.OrdenesDeServicio.Remove(orden);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}