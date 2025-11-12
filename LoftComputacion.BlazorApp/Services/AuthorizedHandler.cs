using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace LoftComputacion.BlazorApp.Services
{
    public class AuthorizedHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthorizedHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                // Le quitamos comillas extra si las hay
                token = token.Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
