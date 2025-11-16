using LoftComputacion.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.WinForms
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "https://localhost:52004/api";

        public string? Token { get; private set; }

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        // ============================================================
        // TOKEN
        // ============================================================
        public void SetToken(string token)
        {
            token = token?.Trim('"');

            Token = token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ============================================================
        // LOGIN
        // ============================================================
        public class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
            public int Id { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string Rol { get; set; } = string.Empty;
        }

        public async Task<LoginResponseDto?> LoginAsync(string nombreUsuario, string password)
        {
            var loginRequest = new { NombreUsuario = nombreUsuario, Password = password };
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/auth/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json_response = await response.Content.ReadAsStringAsync();
            var loginResp = JsonConvert.DeserializeObject<LoginResponseDto>(json_response);

            if (loginResp == null || string.IsNullOrEmpty(loginResp.Token))
                return null;

            SetToken(loginResp.Token);
            return loginResp;
        }

        // ============================================================
        // CLIENTES
        // ============================================================
        public async Task<List<Cliente>> GetClientesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/clientes");

            if (!response.IsSuccessStatusCode)
                return new List<Cliente>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Cliente>>(json) ?? new List<Cliente>();
        }

        public async Task<Cliente> CreateClienteAsync(Cliente nuevoCliente)
        {
            var json = JsonConvert.SerializeObject(nuevoCliente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync($"{_apiUrl}/clientes", content);
            resp.EnsureSuccessStatusCode();

            var jsonResp = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Cliente>(jsonResp);
        }

        // ============================================================
        // EQUIPOS
        // ============================================================
        public async Task<Equipo> CreateEquipoAsync(Equipo eq)
        {
            var json = JsonConvert.SerializeObject(eq);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync($"{_apiUrl}/equipos", content);
            resp.EnsureSuccessStatusCode();

            var jsonResp = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Equipo>(jsonResp);
        }

        // ============================================================
        // ÓRDENES DE SERVICIO
        // ============================================================
        public async Task<List<OrdenDeServicio>> GetOrdenesDeServicioAsync(string? filtro = null)
        {
            string url = $"{_apiUrl}/ordenesdeservicio";

            if (!string.IsNullOrEmpty(filtro))
                url += $"?filtro={Uri.EscapeDataString(filtro)}";

            var resp = await _httpClient.GetAsync(url);

            if (!resp.IsSuccessStatusCode)
                return new List<OrdenDeServicio>();

            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrdenDeServicio>>(json)
                   ?? new List<OrdenDeServicio>();
        }

        public async Task<OrdenDeServicio> CreateOrdenDeServicioAsync(OrdenDeServicio o)
        {
            var dto = new { o.ClienteId, o.EquipoId, o.FallaDeclaradaPorCliente };
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync($"{_apiUrl}/ordenesdeservicio", content);
            resp.EnsureSuccessStatusCode();

            var jsonResp = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrdenDeServicio>(jsonResp);
        }

        public async Task UpdateOrdenDeServicioAsync(int id, OrdenDeServicio o, int usuarioId)
        {
            var dto = new
            {
                o.EstadoId,
                o.PrecioPresupuestado,
                o.PrecioFinal,
                o.ResumenTecnico,
                UsuarioId = usuarioId
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PutAsync($"{_apiUrl}/ordenesdeservicio/{id}", content);
            resp.EnsureSuccessStatusCode();
        }

        // ============================================================
        // HISTORIAL
        // ============================================================
        public async Task<List<HistorialOrden>> GetHistorialDeOrdenAsync(int id)
        {
            var resp = await _httpClient.GetAsync($"{_apiUrl}/ordenesdeservicio/{id}/historial");

            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Error ({resp.StatusCode}): {err}");
            }

            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HistorialOrden>>(json)
                   ?? new List<HistorialOrden>();
        }

        // ============================================================
        // ESTADOS
        // ============================================================
        public async Task<List<Estado>> GetEstadosAsync()
        {
            var resp = await _httpClient.GetAsync($"{_apiUrl}/estados");

            if (!resp.IsSuccessStatusCode)
                return new List<Estado>();

            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Estado>>(json) ?? new List<Estado>();
        }

        // ============================================================
        // USUARIOS
        // ============================================================
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var resp = await _httpClient.GetAsync($"{_apiUrl}/usuarios");
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public async Task<Usuario> CreateUsuarioAsync(string nombre, string email, string pass, string rol)
        {
            var dto = new { NombreCompleto = nombre, Email = email, Password = pass, Rol = rol };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync($"{_apiUrl}/usuarios", content);
            resp.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Usuario>(await resp.Content.ReadAsStringAsync());
        }

        public async Task UpdateUsuarioAsync(int id, string nombre, string email, string rol, string? password)
        {
            var dto = new { NombreCompleto = nombre, Email = email, Rol = rol, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var resp = await _httpClient.PutAsync($"{_apiUrl}/usuarios/{id}", content);
            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            var resp = await _httpClient.DeleteAsync($"{_apiUrl}/usuarios/{id}");
            resp.EnsureSuccessStatusCode();
        }

        // ============================================================
        // FOTOS
        // ============================================================
        public async Task<List<Foto>> GetFotosAsync(int ordenId)
        {
            var resp = await _httpClient.GetAsync($"{_apiUrl}/ordenes/{ordenId}/fotos");
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Foto>>(json) ?? new List<Foto>();
        }

        public async Task<Foto> UploadFotoAsync(int ordenId, Stream image, string fileName)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(image), "file", fileName);

            var resp = await _httpClient.PostAsync($"{_apiUrl}/ordenes/{ordenId}/fotos", content);
            resp.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Foto>(await resp.Content.ReadAsStringAsync());
        }

        public async Task DeleteFotoAsync(int fotoId)
        {
            var resp = await _httpClient.DeleteAsync($"{_apiUrl}/fotos/{fotoId}");
            resp.EnsureSuccessStatusCode();
        }

        // ============================================================
        // GANANCIAS
        // ============================================================
        public class GananciasDto
        {
            public List<OrdenDeServicio> Ordenes { get; set; } = new();
            public decimal Total { get; set; }
        }

        public async Task<GananciasDto> GetGananciasAsync(DateTime desde, DateTime hasta)
        {
            var url = $"{_apiUrl}/ganancias?fechaDesde={desde:yyyy-MM-dd}&fechaHasta={hasta:yyyy-MM-dd}";
            var resp = await _httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<GananciasDto>(await resp.Content.ReadAsStringAsync());
        }

        public async Task<byte[]> DownloadGananciasExcelAsync(DateTime desde, DateTime hasta)
        {
            var url = $"{_apiUrl}/ganancias/exportar?fechaDesde={desde:yyyy-MM-dd}&fechaHasta={hasta:yyyy-MM-dd}";
            var resp = await _httpClient.GetAsync(url);

            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsByteArrayAsync();
        }

        // ============================================================
        // MERCADO PAGO
        // ============================================================
        public class PagoResponseDto
        {
            [JsonProperty("urlDePago")]
            public string UrlDePago { get; set; }
        }

        public async Task<string> CrearLinkDePagoAsync(int ordenId)
        {
            var resp = await _httpClient.PostAsync($"{_apiUrl}/ordenesdeservicio/{ordenId}/crear-pago", null);

            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());

            return JsonConvert.DeserializeObject<PagoResponseDto>(
                await resp.Content.ReadAsStringAsync())?.UrlDePago!;
        }
    }
}
