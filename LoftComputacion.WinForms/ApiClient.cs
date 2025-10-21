using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoftComputacion.Domain;

namespace LoftComputacion.WinForms
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        // ¡OJO AQUÍ! Asegúrate de que el puerto (52004) sea el mismo que usa tu API al ejecutarse.
        private const string _apiUrl = "https://localhost:52004/api";

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/clientes");
            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(json_response);
                return clientes ?? new List<Cliente>();
            }
            return new List<Cliente>();
        }

        // Reemplaza el método existente por este
        public async Task<List<OrdenDeServicio>> GetOrdenesDeServicioAsync(string? filtro = null) // Agregamos el parámetro opcional
        {
            string url = $"{_apiUrl}/ordenesdeservicio";
            if (!string.IsNullOrEmpty(filtro))
            {
                // Si hay un filtro, lo agregamos a la URL como un 'query parameter'
                url += $"?filtro={Uri.EscapeDataString(filtro)}";
            }

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var ordenes = JsonConvert.DeserializeObject<List<OrdenDeServicio>>(json_response);
                return ordenes ?? new List<OrdenDeServicio>();
            }
            return new List<OrdenDeServicio>();
        }

        public async Task<Cliente> CreateClienteAsync(Cliente nuevoCliente)
        {
            var json = JsonConvert.SerializeObject(nuevoCliente);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/clientes", content);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            return  JsonConvert.DeserializeObject<Cliente>(json_response);
        }

        public async Task<Equipo> CreateEquipoAsync(Equipo nuevoEquipo)
        {
            var json = JsonConvert.SerializeObject(nuevoEquipo);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/equipos", content);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Equipo>(json_response);
        }

        public async Task<OrdenDeServicio> CreateOrdenDeServicioAsync(OrdenDeServicio nuevaOrden)
        {
            // La API espera un DTO, así que creamos un objeto anónimo con la estructura correcta
            var createDto = new { nuevaOrden.ClienteId, nuevaOrden.EquipoId, nuevaOrden.FallaDeclaradaPorCliente };
            var dto_json = JsonConvert.SerializeObject(createDto);
            var dto_content = new StringContent(dto_json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/ordenesdeservicio", dto_content);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrdenDeServicio>(json_response);
        }

        // Asegúrate de que el método reciba el usuarioId
        public async Task UpdateOrdenDeServicioAsync(int id, OrdenDeServicio ordenActualizada, int usuarioId)
        {
            var updateDto = new
            {
                ordenActualizada.EstadoId,
                ordenActualizada.PrecioPresupuestado,
                ordenActualizada.PrecioFinal,
                UsuarioId = usuarioId // Usamos el ID recibido
            };
            var dto_json = JsonConvert.SerializeObject(updateDto);
            var dto_content = new StringContent(dto_json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/ordenesdeservicio/{id}", dto_content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Estado>> GetEstadosAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/estados");
            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var estados = JsonConvert.DeserializeObject<List<Estado>>(json_response);
                return estados ?? new List<Estado>();
            }
            return new List<Estado>();
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/usuarios");
            response.EnsureSuccessStatusCode();
            var json_response = await response.Content.ReadAsStringAsync();
            var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json_response);
            return usuarios ?? new List<Usuario>();
        }
    }
}