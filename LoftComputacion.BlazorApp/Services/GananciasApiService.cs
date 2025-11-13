// LoftComputacion.BlazorApp/Services/GananciasApiService.cs

using LoftComputacion.BlazorApp.DTOs;
using LoftComputacion.Domain;
using System.Net.Http.Json;

namespace LoftComputacion.BlazorApp.Services
{
    public class GananciasApiService
    {
        private readonly HttpClient _httpClient;

        public GananciasApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para obtener el reporte resumen
        public async Task<IEnumerable<OrdenDeServicio>?> GetReporteAsync(DateTime inicio, DateTime fin)
        {
            string url = $"api/ganancias/reporte?inicio={inicio:yyyy-MM-dd}&fin={fin:yyyy-MM-dd}";

            try
            {
                // Esperamos una LISTA de órdenes
                var reporte = await _httpClient.GetFromJsonAsync<IEnumerable<OrdenDeServicio>>(url);
                return reporte;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el reporte de ganancias: {ex.Message}");
                return null;
            }
        }

        // Método para descargar el Excel (devuelve los bytes)
        public async Task<byte[]?> DescargarExcelAsync(DateTime inicio, DateTime fin)
        {
            string url = $"api/ganancias/excel?inicio={inicio:yyyy-MM-dd}&fin={fin:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Lanza excepción si el código no es 2xx

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}