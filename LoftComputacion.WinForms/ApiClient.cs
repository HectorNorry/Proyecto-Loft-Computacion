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

        public async Task<List<OrdenDeServicio>> GetOrdenesDeServicioAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/ordenesdeservicio");
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

        public async Task UpdateOrdenDeServicioAsync(int id, OrdenDeServicio ordenActualizada)
        {
            // La API espera un DTO específico para actualizar
            var updateDto = new
            {
                ordenActualizada.EstadoId,
                ordenActualizada.PrecioPresupuestado,
                ordenActualizada.PrecioFinal,
                // TODO: Obtener el ID del usuario logueado o seleccionado
                UsuarioId = 1 // Por ahora, usamos el ID 1 como ejemplo
            };
            var dto_json = JsonConvert.SerializeObject(updateDto);
            var dto_content = new StringContent(dto_json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/ordenesdeservicio/{id}", dto_content);
            response.EnsureSuccessStatusCode(); // Lanza excepción si la API devuelve error
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
    }
}