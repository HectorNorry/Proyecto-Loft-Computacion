using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.Application
{
    public class OrdenDeServicioService
    {
        private readonly ApplicationDbContext _context;
        private readonly AIService _aiService;

        public OrdenDeServicioService(ApplicationDbContext context, AIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public async Task<IEnumerable<OrdenDeServicio>> GetAllOrdenesAsync(string? filtro = null) // Agregamos el parámetro
        {
            // Empezamos con la consulta base, incluyendo los datos relacionados
            var query = _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .AsQueryable(); // Importante: AsQueryable() permite añadir filtros después

            if (!string.IsNullOrEmpty(filtro))
            {
                var filtroLower = filtro.ToLowerInvariant();
                query = query.Where(o =>
                    o.Id.ToString() == filtroLower || // Busca por ID de orden (si es número exacto)
                    (o.Cliente != null && o.Cliente.NombreCompleto.ToLowerInvariant().Contains(filtroLower)) || // Busca en nombre cliente
                    (o.Cliente != null && o.Cliente.DNI != null && o.Cliente.DNI.Contains(filtroLower)) || // Busca en DNI cliente
                    (o.Equipo != null && o.Equipo.NumeroDeSerie != null && o.Equipo.NumeroDeSerie.ToLowerInvariant().Contains(filtroLower)) // Busca en Nro Serie Equipo
                );
            }

            // Solo al final ejecutamos la consulta con ToListAsync()
            return await query.ToListAsync();
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

            var estadoAnterior = await _context.Estados.FindAsync(ordenExistente.EstadoId);
            var estadoNuevo = await _context.Estados.FindAsync(ordenActualizada.EstadoId);

            var descripcionCambio = $"El estado cambió de '{estadoAnterior?.Nombre ?? "Desconocido"}' a '{estadoNuevo?.Nombre ?? "Desconocido"}'.";

            if (estadoNuevo?.Nombre == "Finalizado, a espera de pago")
            {
                var trabajoRealizado = "Se reemplazó el disco duro por un SSD y se reinstaló el sistema operativo.";
                var resumenParaCliente = await _aiService.GenerarResumenAsync(trabajoRealizado);
                descripcionCambio += $" Resumen para cliente: {resumenParaCliente}";
            }

            var historial = new HistorialOrden
            {
                OrdenDeServicioId = id,
                UsuarioId = usuarioId,
                FechaHora = DateTime.UtcNow,
                DescripcionDelCambio = descripcionCambio
            };
            await _context.HistorialOrdenes.AddAsync(historial);

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