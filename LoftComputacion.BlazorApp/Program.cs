using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
// Asegúrate de que estos namespaces coincidan con tus carpetas reales
using LoftComputacion.BlazorApp;
using LoftComputacion.BlazorApp.Services.Auth;
using LoftComputacion.BlazorApp.Services.Ordenes;
using LoftComputacion.BlazorApp.Services.Clientes;
using LoftComputacion.BlazorApp.Services.Ai;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// =================================================================
// 1. CONFIGURACIÓN DE CULTURA
// =================================================================
var culture = new CultureInfo("es-AR");
culture.NumberFormat.CurrencySymbol = "$";
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// =================================================================
// 2. URL DINÁMICA (CORREGIDO)
// =================================================================
// Al usar la propiedad BaseAddress del HostEnvironment, la URL se adapta sola.
// En Local será: https://localhost:TUPUERTO/
// En Producción será: https://tu-sitio.azurewebsites.net/
//string backendUrl = builder.HostEnvironment.BaseAddress;
string backendUrl = "https://loftcomputacion-api-webapp-ceeycjhrb9evfvbj.brazilsouth-01.azurewebsites.net/";

// Registramos el Handler que pega el Token en las llamadas
builder.Services.AddTransient<JwtAuthMessageHandler>();

// Cliente HTTP genérico
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendUrl) });

// =================================================================
// 3. SERVICIOS DE NEGOCIO (Tipados)
// =================================================================

// Auth (No lleva Token Handler porque es para loguearse)
builder.Services.AddHttpClient<AuthApiService>(client =>
    client.BaseAddress = new Uri(backendUrl));

// Órdenes (Lleva Token)
builder.Services.AddHttpClient<OrdenesApiService>(client =>
    client.BaseAddress = new Uri(backendUrl))
    .AddHttpMessageHandler<JwtAuthMessageHandler>();

// Clientes (Lleva Token)
builder.Services.AddHttpClient<ClientesApiService>(client =>
    client.BaseAddress = new Uri(backendUrl))
    .AddHttpMessageHandler<JwtAuthMessageHandler>();

// Usuarios (Lleva Token)
builder.Services.AddHttpClient<UsuariosApiService>(client =>
    client.BaseAddress = new Uri(backendUrl))
    .AddHttpMessageHandler<JwtAuthMessageHandler>();

// Inteligencia Artificial (Lleva Token)
builder.Services.AddHttpClient<AiApiService>(client =>
    client.BaseAddress = new Uri(backendUrl))
    .AddHttpMessageHandler<JwtAuthMessageHandler>();



// =================================================================
// 4. AUTENTICACIÓN Y UI
// =================================================================
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

// Proveedor de Estado de Auth
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

// Librería Gráfica MudBlazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();