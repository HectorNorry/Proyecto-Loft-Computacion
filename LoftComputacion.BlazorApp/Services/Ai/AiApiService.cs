using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

// IMPORTANTE: Este namespace debe coincidir con lo que espera el Program.cs
namespace LoftComputacion.BlazorApp.Services.Ai
{
    public class AiApiService
    {
        private readonly HttpClient _http;

        public AiApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HttpResponseMessage> ConsultarExperto(string promptUsuario)
        {
            // Llama al controlador que creamos en la API
            // Enviamos un objeto anónimo con la propiedad "prompt"
            return await _http.PostAsJsonAsync("api/ia/consultar", new { prompt = promptUsuario });
        }
    }
}