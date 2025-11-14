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
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// --- Handler que agrega el token JWT automáticamente ---
builder.Services.AddTransient<AuthorizedHandler>();

// --- HttpClient que usa el AuthorizedHandler ---
builder.Services.AddScoped(sp =>
{
    var localStorage = sp.GetRequiredService<ILocalStorageService>();
    var handler = new AuthorizedHandler(localStorage);

    // 🚨 CORRECCIÓN CLAVE: Se elimina el HttpClientHandler (que causaba el crash)
    handler.InnerHandler = new HttpClientHandler();

    var httpClient = new HttpClient(handler)
    {
        // 🚨 CORRECCIÓN CLAVE: Usamos la URL base del entorno de hosting
        // Como ahora la API sirve la app, la dirección es la misma.
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
    return httpClient;
});

// --- Servicios que consumen la API ---
builder.Services.AddScoped<OrdenesApiService>();
builder.Services.AddScoped<ClientesApiService>();
builder.Services.AddScoped<UsuarioApiService>();
builder.Services.AddScoped<GananciasApiService>();

await builder.Build().RunAsync();