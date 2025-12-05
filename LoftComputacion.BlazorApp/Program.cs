using Blazored.LocalStorage;
using LoftComputacion.BlazorApp;
using LoftComputacion.BlazorApp.Services.Auth;
using LoftComputacion.BlazorApp.Services.Clientes;
using LoftComputacion.BlazorApp.Services.Ordenes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;

// ============================================
// 1) Creamos cultura ES-AR para usar el $
// ============================================
var culture = new CultureInfo("es-AR");
culture.NumberFormat.CurrencySymbol = "$";  // fuerza símbolo de $
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ============================================
// 2) HttpClient simple apuntando a la API
// ============================================
// Registrar el Handler como servicio (para que pueda ser inyectado)
builder.Services.AddTransient<JwtAuthMessageHandler>();

// Configurar el cliente HTTP nombrado "API"
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://25bkxsbn-7081.brs.devtunnels.ms/"); // Tu puerto de API


})


    .AddHttpMessageHandler<JwtAuthMessageHandler>(); // ¡Aquí está la magia!

// Crear el cliente default usando la fábrica
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));
// ============================================
// 3) Auth + LocalStorage
// ============================================
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

// --- CORRECCIÓN AQUÍ ---
// 1. Registramos tu clase concreta
builder.Services.AddScoped<CustomAuthStateProvider>();

// 2. Le decimos al sistema: "Cuando necesites AuthenticationStateProvider, 
// usa la MISMA instancia de CustomAuthStateProvider que creamos arriba"
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

// ============================================
// 4) Servicios propios
// ============================================
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<OrdenesApiService>();
builder.Services.AddScoped<ClientesApiService>();
builder.Services.AddHttpClient<UsuariosApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:52004/"); // Asegúrate que este sea tu puerto API
})
    .AddHttpMessageHandler<JwtAuthMessageHandler>();



// ============================================
// 5) MudBlazor
// ============================================
builder.Services.AddMudServices();

await builder.Build().RunAsync();
