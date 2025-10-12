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

            // Creamos el registro de historial ANTES de hacer el cambio
            var historial = new HistorialOrden
            {
                OrdenDeServicioId = id,
                UsuarioId = usuarioId, // El ID del usuario que hace el cambio
                FechaHora = DateTime.UtcNow,
                DescripcionDelCambio = $"El estado cambió de '{ordenExistente.EstadoId}' a '{ordenActualizada.EstadoId}'."
                // En el futuro podemos hacer esto más amigable buscando el nombre del estado
            };

            await _context.HistorialOrdenes.AddAsync(historial);

            // Ahora, actualizamos los campos de la orden
            ordenExistente.EstadoId = ordenActualizada.EstadoId;
            ordenExistente.PrecioPresupuestado = ordenActualizada.PrecioPresupuestado;
            ordenExistente.PrecioFinal = ordenActualizada.PrecioFinal;

            await _context.SaveChangesAsync(); // Guardamos ambos cambios (la orden y el historial) en una sola transacción
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