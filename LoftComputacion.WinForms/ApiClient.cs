using LoftComputacion.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace LoftComputacion.WinForms
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        private const string _apiUrl = "https://localhost:52004/api";

        private static string? _jwtToken;

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

               public void SetToken(string token)
        {
            // Agrega el token a los encabezados por defecto
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
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

        // Nuevo método para obtener las ganancias
        public async Task<GananciasDto> GetGananciasAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            // Formateamos las fechas al formato YYYY-MM-DD que espera la API
            string fechaDesdeStr = fechaDesde.ToString("yyyy-MM-dd");
            string fechaHastaStr = fechaHasta.ToString("yyyy-MM-dd");

            string url = $"{_apiUrl}/ganancias?fechaDesde={fechaDesdeStr}&fechaHasta={fechaHastaStr}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Lanza excepción si hay error

            var json_response = await response.Content.ReadAsStringAsync();

            // Creamos una clase temporal para deserializar la respuesta completa (lista + total)
            var resultado = JsonConvert.DeserializeObject<GananciasDto>(json_response);

            return resultado ?? new GananciasDto(); // Devolvemos el objeto o uno vacío si falla
        }

        
        public class GananciasDto
        {
            public List<OrdenDeServicio> Ordenes { get; set; } = new List<OrdenDeServicio>();
            public decimal Total { get; set; }
        }

        public async Task<byte[]> DownloadGananciasExcelAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            // Formateamos las fechas al formato YYYY-MM-DD
            string fechaDesdeStr = fechaDesde.ToString("yyyy-MM-dd");
            string fechaHastaStr = fechaHasta.ToString("yyyy-MM-dd");

            string url = $"{_apiUrl}/ganancias/exportar?fechaDesde={fechaDesdeStr}&fechaHasta={fechaHastaStr}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Lanza excepción si hay error

            // Leemos la respuesta no como texto/json, sino como un array de bytes
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return fileBytes;
        }

        // Método para OBTENER la lista de fotos de una orden
        public async Task<List<Foto>> GetFotosAsync(int ordenId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/ordenes/{ordenId}/fotos"); // <-- ¡Endpoint nuevo!
            response.EnsureSuccessStatusCode();
            var json_response = await response.Content.ReadAsStringAsync();
            var fotos = JsonConvert.DeserializeObject<List<Foto>>(json_response);
            return fotos ?? new List<Foto>();
        }

        // Método para SUBIR una foto (comprimida)
        public async Task<Foto> UploadFotoAsync(int ordenId, Stream imageStream, string fileName)
        {
            // Usamos MultipartFormDataContent para enviar archivos
            using (var content = new MultipartFormDataContent())
            {
                // "fileStream" es el contenido (bytes) de la imagen
                // "file" es el nombre que espera la API (IFormFile file)
                // "fileName" es el nombre del archivo
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
                // Descarga los bytes de la imagen
                byte[] imageData = await _httpClient.GetByteArrayAsync(url);
                // Convierte los bytes en un objeto Image
                using (var ms = new MemoryStream(imageData))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null; // Devuelve null si la descarga falla
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
                // Deserializa la respuesta completa
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(json_response);

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    // Guardamos el token en el cliente
                    SetToken(loginResponse.Token);
                    return loginResponse; // Devolvemos el objeto completo
                }
                return null;
            }
            else
            {
                return null; // Falla el login
            }
        }

       

        private class TokenDto { public string Token { get; set; } = string.Empty; }

        public async Task DeleteUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/usuarios/{usuarioId}");
            response.EnsureSuccessStatusCode(); // Lanzará una excepción si la API devuelve un error (ej: 401, 404, 500)
        }

        public async Task<Usuario> CreateUsuarioAsync(string nombreCompleto, string email, string password, string rol)
        {
            // 1. Creamos un objeto anónimo que tiene la "forma" del DTO que espera la API
            var nuevoUsuarioDto = new
            {
                NombreCompleto = nombreCompleto,
                Email = email,
                Password = password,
                Rol = rol
            };

            // 2. Lo convertimos a JSON
            var json = JsonConvert.SerializeObject(nuevoUsuarioDto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // 3. Lo enviamos al endpoint
            var response = await _httpClient.PostAsync($"{_apiUrl}/usuarios", content);
            response.EnsureSuccessStatusCode();

            var json_response = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Usuario>(json_response);
        }

        public async Task UpdateUsuarioAsync(int usuarioId, string nombre, string email, string rol, string? password)
        {
            // Creamos un objeto anónimo que coincide con el UpdateUsuarioDto
            var updateDto = new
            {
                NombreCompleto = nombre,
                Email = email,
                Rol = rol,
                Password = password // Será null si está vacío, lo cual es perfecto
            };

            var json = JsonConvert.SerializeObject(updateDto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/usuarios/{usuarioId}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}