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
                        ordenExistente.PrecioFinal ?? 0,               // 4. precioFinal
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