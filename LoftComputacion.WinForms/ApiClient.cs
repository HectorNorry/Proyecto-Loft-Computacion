using LoftComputacion.Domain;
using Newtonsoft.Json;
using System; // Agregado para Uri
using System.Collections.Generic;
using System.Drawing; // Para Image
using System.IO; // Para MemoryStream
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LoftComputacion.WinForms
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        // Esta es la URL correcta que ya tenías
        private const string _apiUrl = "https://localhost:52004/api";

        private static string? _jwtToken;


        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public void SetToken(string token)
        {
            _jwtToken = token; 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
            public int Id { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string Rol { get; set; } = string.Empty;
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

        public async Task<List<OrdenDeServicio>> GetOrdenesDeServicioAsync(string? filtro = null)
        {
            string url = $"{_apiUrl}/ordenesdeservicio";
            if (!string.IsNullOrEmpty(filtro))
            {
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
            return JsonConvert.DeserializeObject<Cliente>(json_response);
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
            var createDto = new { nuevaOrden.ClienteId, nuevaOrden.EquipoId, nuevaOrden.FallaDeclaradaPorCliente };
            var dto_json = JsonConvert.SerializeObject(createDto);
            var dto_content = new StringContent(dto_json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/ordenesdeservicio", dto_content);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrdenDeServicio>(json_response);
        }

        public async Task UpdateOrdenDeServicioAsync(int id, OrdenDeServicio ordenActualizada, int usuarioId)
        {
            var updateDto = new
            {
                ordenActualizada.EstadoId,
                ordenActualizada.PrecioPresupuestado,
                ordenActualizada.PrecioFinal,
                ordenActualizada.ResumenTecnico,
                UsuarioId = usuarioId
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

        public async Task<GananciasDto> GetGananciasAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            string fechaDesdeStr = fechaDesde.ToString("yyyy-MM-dd");
            string fechaHastaStr = fechaHasta.ToString("yyyy-MM-dd");

            string url = $"{_apiUrl}/ganancias?fechaDesde={fechaDesdeStr}&fechaHasta={fechaHastaStr}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<GananciasDto>(json_response);

            return resultado ?? new GananciasDto();
        }

        public class GananciasDto
        {
            public List<OrdenDeServicio> Ordenes { get; set; } = new List<OrdenDeServicio>();
            public decimal Total { get; set; }
        }

        public async Task<byte[]> DownloadGananciasExcelAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            string fechaDesdeStr = fechaDesde.ToString("yyyy-MM-dd");
            string fechaHastaStr = fechaHasta.ToString("yyyy-MM-dd");

            string url = $"{_apiUrl}/ganancias/exportar?fechaDesde={fechaDesdeStr}&fechaHasta={fechaHastaStr}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return fileBytes;
        }

        public async Task<List<Foto>> GetFotosAsync(int ordenId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/ordenes/{ordenId}/fotos");
            response.EnsureSuccessStatusCode();
            var json_response = await response.Content.ReadAsStringAsync();
            var fotos = JsonConvert.DeserializeObject<List<Foto>>(json_response);
            return fotos ?? new List<Foto>();
        }

        public async Task<Foto> UploadFotoAsync(int ordenId, Stream imageStream, string fileName)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(imageStream), "file", fileName);

                var response = await _httpClient.PostAsync($"{_apiUrl}/ordenes/{ordenId}/fotos", content);
                response.EnsureSuccessStatusCode();

                var json_response = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Foto>(json_response);
            }
        }

        public async Task<Image?> DownloadImageAsync(string url)
        {
            try
            {
                byte[] imageData = await _httpClient.GetByteArrayAsync(url);
                using (var ms = new MemoryStream(imageData))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task DeleteFotoAsync(int fotoId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/fotos/{fotoId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<LoginResponseDto?> LoginAsync(string nombreUsuario, string password)
        {
            var loginRequest = new { NombreUsuario = nombreUsuario, Password = password };
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/usuarios/login", content);

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(json_response);

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    SetToken(loginResponse.Token);
                    return loginResponse;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private class TokenDto { public string Token { get; set; } = string.Empty; }

        public async Task DeleteUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/usuarios/{usuarioId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Usuario> CreateUsuarioAsync(string nombreCompleto, string email, string password, string rol)
        {
            var nuevoUsuarioDto = new
            {
                NombreCompleto = nombreCompleto,
                Email = email,
                Password = password,
                Rol = rol
            };
            var json = JsonConvert.SerializeObject(nuevoUsuarioDto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}/usuarios", content);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Usuario>(json_response);
        }

        public async Task UpdateUsuarioAsync(int usuarioId, string nombre, string email, string rol, string? password)
        {
            var updateDto = new
            {
                NombreCompleto = nombre,
                Email = email,
                Rol = rol,
                Password = password
            };

            var json = JsonConvert.SerializeObject(updateDto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/usuarios/{usuarioId}", content);
            response.EnsureSuccessStatusCode();
        }

        // --- MÉTODO DEL HISTORIAL CORREGIDO ---
        public async Task<List<HistorialOrden>> GetHistorialDeOrdenAsync(int ordenId)
        {
            SetToken(_jwtToken); // Nos aseguramos que el token esté

            // --- 1. CORRECCIÓN DE URL ---
            // Añadimos la variable _apiUrl como en el resto de tus métodos
            // y usamos el endpoint correcto de la API (ordenesdeservicio)
            var response = await _httpClient.GetAsync($"{_apiUrl}/ordenesdeservicio/{ordenId}/historial");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener el historial ({response.StatusCode}): {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // --- 2. CORRECCIÓN DE JSON ---
            // Usamos Newtonsoft (JsonConvert) para ser consistentes con el resto del archivo
            var historial = JsonConvert.DeserializeObject<List<HistorialOrden>>(jsonResponse);
            return historial ?? new List<HistorialOrden>();
        }

        // --- AGREGAR ESTE MÉTODO NUEVO DENTRO DE ApiClient.cs ---

        /// <summary>
        /// Llama a la API para crear una preferencia de pago en Mercado Pago.
        /// </summary>
        /// <param name="ordenId">El ID de la orden a pagar</param>
        /// <returns>La URL del checkout de Mercado Pago para abrir en el navegador.</returns>
        public async Task<string> CrearLinkDePagoAsync(int ordenId)
        {
            SetToken(_jwtToken); // Nos aseguramos que el token esté

            // 1. Llamamos al nuevo endpoint. Usamos un 'HttpContent' vacío
            //    porque el ID va en la URL y no enviamos datos en el body.
            var response = await _httpClient.PostAsync($"{_apiUrl}/ordenesdeservicio/{ordenId}/crear-pago", null);

            if (!response.IsSuccessStatusCode)
            {
                // Si la API da un error (ej: 400 "No hay precio"), lo leemos y lo mostramos
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear el link de pago: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // 2. Leemos el JSON anónimo que nos devuelve el controlador
            //    (Necesitamos una clase helper para leer la 'urlDePago')
            var resultado = JsonConvert.DeserializeObject<PagoResponseDto>(jsonResponse);

            if (resultado == null || string.IsNullOrEmpty(resultado.UrlDePago))
            {
                throw new Exception("La API no devolvió una URL de pago válida.");
            }

            return resultado.UrlDePago;
        }

        
    }
    /// <summary>
    /// Clase helper para leer la respuesta de la API al crear un link de pago.
    /// </summary>
    public class PagoResponseDto
    {
        // Le decimos a Newtonsoft que busque "urlDePago" (con minúscula) en el JSON
        // para que coincida con el 'return Ok(new { urlDePago = ... })' del controlador.
        [JsonProperty("urlDePago")]
        public string UrlDePago { get; set; }
    }
}