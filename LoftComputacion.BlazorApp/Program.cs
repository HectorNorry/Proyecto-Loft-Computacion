// --- Program.cs FINAL para LoftComputacion.BlazorApp ---

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using LoftComputacion.BlazorApp.Services;
using Microsoft.AspNetCore.Components.Web;
using LoftComputacion.BlazorApp;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// --- Componentes raíz ---
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// --- Servicios base ---
builder.Services.AddBlazoredLocalStorage(); // Para guardar el token JWT
builder.Services.AddAuthorizationCore();    // Para [Authorize]
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// --- Handler que agrega el token JWT automáticamente ---
builder.Services.AddTransient<AuthorizedHandler>();

// --- HttpClient que usa el AuthorizedHandler ---
builder.Services.AddScoped(sp =>
{
    var localStorage = sp.GetRequiredService<ILocalStorageService>();
    var handler = new AuthorizedHandler(localStorage);

    // --- ¡AQUÍ VA LA LÍNEA QUE FALTA! ---
    // Le decimos al handler que, después de que él termine (de poner el token),
    // la llamada debe continuar al "manejador base" de HttpClient.
    handler.InnerHandler = new HttpClientHandler();
    // ----------------------------------------

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("https://localhost:52004") // URL de tu API
    };
    return httpClient;
});

// --- Servicio que consume la API de órdenes ---
builder.Services.AddScoped<OrdenesApiService>();

await builder.Build().RunAsync();
