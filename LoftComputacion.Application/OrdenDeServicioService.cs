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


        public async Task<IEnumerable<OrdenDeServicio>> GetAllOrdenesAsync(string? filtro)
        {
            // ... (Tu código de Get con filtro)
            var query = _context.OrdenesDeServicio
                                .Include(o => o.Cliente)
                                .Include(o => o.Equipo)
                                .Include(o => o.Estado)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(o => o.Cliente.NombreCompleto.Contains(filtro) || o.Equipo.Modelo.Contains(filtro) || o.Id.ToString() == filtro);
            }

            return await query.ToListAsync();
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
            // ... (Tu código de Create)
            // Asignar estado inicial (Ej: "Recibido")
            orden.EstadoId = 1; // Asumo 1 = Recibido

            // --- ¡CORRECCIÓN! ---
            // Se elimina la siguiente línea porque la propiedad "FechaCreacion" no existe
            // orden.FechaCreacion = DateTime.UtcNow;

            _context.OrdenesDeServicio.Add(orden);
            await _context.SaveChangesAsync();
            return orden;
        }


        // --- ¡AQUÍ ESTÁ TODA LA LÓGICA NUEVA! ---
        public async Task<bool> UpdateOrdenAsync(int id, OrdenDeServicio ordenConNuevosDatos, int usuarioId)
        {
            var ordenExistente = await _context.OrdenesDeServicio
                .Include(o => o.Cliente) // ¡Importante incluir al Cliente para saber su email!
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordenExistente == null)
            {
                return false; // No se encontró la orden
            }

            // Guardamos el estado anterior para compararlo
            int estadoAnterior = ordenExistente.EstadoId;
            int estadoNuevo = ordenConNuevosDatos.EstadoId;

            // Actualizamos la orden en la base de datos
            ordenExistente.EstadoId = estadoNuevo;
            ordenExistente.PrecioPresupuestado = ordenConNuevosDatos.PrecioPresupuestado;
            ordenExistente.PrecioFinal = ordenConNuevosDatos.PrecioFinal;
            ordenExistente.ResumenTecnico = ordenConNuevosDatos.ResumenTecnico; // ¡Guardamos el resumen!

            // (Aquí puedes agregar la lógica del historial de cambios si la tienes)

            await _context.SaveChangesAsync();

            // --- LÓGICA DE NOTIFICACIÓN POR IA ---
            // Comprobamos si el estado cambió a uno "Finalizado"
            // (¡AJUSTA EL ID_ESTADO_FINALIZADO_ESPERA_PAGO!)
            if (estadoNuevo == ID_ESTADO_FINALIZADO_ESPERA_PAGO && estadoAnterior != ID_ESTADO_FINALIZADO_ESPERA_PAGO)
            {
                string resumenParaEmail = "";

                // Verificamos si el técnico escribió un resumen
                if (!string.IsNullOrWhiteSpace(ordenExistente.ResumenTecnico))
                {
                    // CASO 1: Usamos el método de "traducción"
                    resumenParaEmail = await _aiService.GenerarResumenDesdeTecnicoAsync(ordenExistente.ResumenTecnico);
                }
                else
                {
                    // CASO 2: Usamos el método "genérico"
                    resumenParaEmail = await _aiService.GenerarResumenDesdeFallaAsync(ordenExistente.FallaDeclaradaPorCliente);
                }

                // Si la IA falló o devolvió null, ponemos un mensaje por defecto
                if (string.IsNullOrEmpty(resumenParaEmail))
                {
                    resumenParaEmail = "¡Tu equipo está listo y funcionando correctamente! Ya puedes pasar a retirarlo.\n\nSaludos,\nEl equipo de LOFT COMPUTACIÓN";
                }

                // Enviamos el email
                // (La variable 'asuntoEmail' se elimina, ya que EmailService define su propio asunto)

                // Nos aseguramos de que el cliente y su email existan
                if (ordenExistente.Cliente != null && !string.IsNullOrEmpty(ordenExistente.Cliente.Email))
                {

                    // --- ¡CORRECCIÓN 3: Añadir el 'ordenExistente.Id' al final! ---
                    await _emailService.EnviarEmailNotificacion(
                        ordenExistente.Cliente.Email,                  // 1. emailCliente
                        ordenExistente.Cliente.NombreCompleto,         // 2. nombreCliente
                        resumenParaEmail,                              // 3. resumenIA
                        ordenExistente.Id                              // 5. ordenId (¡NUEVO!)
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