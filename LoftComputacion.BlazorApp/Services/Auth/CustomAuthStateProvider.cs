using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Diagnostics;

namespace LoftComputacion.BlazorApp.Services.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity())
            );
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = "";

            try
            {
                // INTENTO 1: Lectura normal
                token = await _localStorage.GetItemAsync<string>("authToken");
            }
            catch
            {
                // Si falla (error de JS interop o timing), esperamos 100ms y reintentamos
                // Esto soluciona problemas de arranque en frío
                await Task.Delay(100);
                try
                {
                    token = await _localStorage.GetItemAsync<string>("authToken");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>> AUTH ERROR: No se pudo leer del LocalStorage: {ex.Message}");
                }
            }

            // Si después del reintento sigue vacío, devolvemos anónimo
            if (string.IsNullOrWhiteSpace(token))
            {
                // Console.WriteLine(">>> AUTH: Token vacío o nulo. Usuario anónimo.");
                return _anonymous;
            }

            // LIMPIEZA: Quitamos comillas extras si existen
            token = token.Trim('"');

            // VALIDACIÓN DEL JWT
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // Si llegamos aquí, el token tiene formato válido
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                // Console.WriteLine(">>> AUTH: ¡Usuario autenticado correctamente!");
                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> AUTH FATAL: El token existe pero es inválido. Error: {ex.Message}");
                Console.WriteLine($">>> TOKEN DEFECTUOSO: {token}");

                // Si el token es basura, lo borramos para no quedar en bucle
                await _localStorage.RemoveItemAsync("authToken");
                return _anonymous;
            }
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            // Guardamos el token limpio
            await _localStorage.SetItemAsync("authToken", token.Trim('"'));

            // Notificamos a la app que el estado cambió
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        public async Task<int> GetUserIdAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            var user = authState.User;

            var idClaim = user.FindFirst("sub")?.Value ??
                          user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(idClaim, out int userId))
                return userId;

            return -1;
        }
    }
}