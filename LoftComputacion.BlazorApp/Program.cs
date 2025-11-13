// --- Program.cs SIMPLE para LoftComputacion.BlazorApp ---

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

    // 🚨 Dejamos solo el handler base (sin el bypass de SSL)
    handler.InnerHandler = new HttpClientHandler();

    var httpClient = new HttpClient(handler)
    {
        // Apuntamos a la API local
        BaseAddress = new Uri("https://localhost:52004")
    };
    return httpClient;
});

// --- Servicios que consumen la API ---
builder.Services.AddScoped<OrdenesApiService>();
builder.Services.AddScoped<ClientesApiService>();
builder.Services.AddScoped<UsuarioApiService>();
builder.Services.AddScoped<GananciasApiService>();

await builder.Build().RunAsync();