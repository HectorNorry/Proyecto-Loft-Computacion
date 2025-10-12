using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.Application
{
    public class OrdenDeServicioService
    {
        private readonly ApplicationDbContext _context;

        public OrdenDeServicioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenDeServicio>> GetAllOrdenesAsync()
        {
            return await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .ToListAsync();
        }

        public async Task<OrdenDeServicio?> GetOrdenByIdAsync(int id)
        {
            return await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrdenDeServicio> CreateOrdenAsync(OrdenDeServicio nuevaOrden)
        {
            // Asignamos el estado inicial. En el futuro, lo buscaremos en la BD.
            nuevaOrden.EstadoId = 1; // "Recibido"
            nuevaOrden.FechaIngreso = DateTime.UtcNow;

            await _context.OrdenesDeServicio.AddAsync(nuevaOrden);
            await _context.SaveChangesAsync();
            return nuevaOrden;
        }

        public async Task<bool> UpdateOrdenAsync(int id, OrdenDeServicio ordenActualizada, int usuarioId)
        {
            var ordenExistente = await _context.OrdenesDeServicio.FindAsync(id);
            if (ordenExistente == null)
            {
                return false;
            }

            // --- LÓGICA DE AUDITORÍA MEJORADA ---

            // 1. Buscamos los nombres de los estados en la base de datos
            var estadoAnterior = await _context.Estados.FindAsync(ordenExistente.EstadoId);
            var estadoNuevo = await _context.Estados.FindAsync(ordenActualizada.EstadoId);

            // 2. Creamos una descripción más amigable
            var descripcionCambio = $"El estado cambió de '{estadoAnterior?.Nombre ?? "Desconocido"}' a '{estadoNuevo?.Nombre ?? "Desconocido"}'.";

            var historial = new HistorialOrden
            {
                OrdenDeServicioId = id,
                UsuarioId = usuarioId,
                FechaHora = DateTime.UtcNow,
                DescripcionDelCambio = descripcionCambio
            };
            await _context.HistorialOrdenes.AddAsync(historial);

            // --- FIN DE LA LÓGICA DE AUDITORÍA ---

            // Ahora, actualizamos los campos de la orden
            ordenExistente.EstadoId = ordenActualizada.EstadoId;
            ordenExistente.PrecioPresupuestado = ordenActualizada.PrecioPresupuestado;
            ordenExistente.PrecioFinal = ordenActualizada.PrecioFinal;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrdenAsync(int id)
        {
            var ordenExistente = await _context.OrdenesDeServicio.FindAsync(id);
            if (ordenExistente == null)
            {
                return false;
            }

            _context.OrdenesDeServicio.Remove(ordenExistente);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}