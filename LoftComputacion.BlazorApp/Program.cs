using Blazored.LocalStorage;
using LoftComputacion.BlazorApp;
using LoftComputacion.BlazorApp.Services.Auth;
using LoftComputacion.BlazorApp.Services.Clientes;
using LoftComputacion.BlazorApp.Services.Ordenes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
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
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://loftcomputacion-api-webapp-ceeyjhrb9fvbj.brazilsouth-01.azurewebsites.net/") });

// ============================================
// 3) Auth + LocalStorage
// ============================================
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();

// ============================================
// 4) Servicios propios
// ============================================
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<OrdenesApiService>();
builder.Services.AddScoped<ClientesApiService>();
builder.Services.AddScoped<UsuariosApiService>();



// ============================================
// 5) MudBlazor
// ============================================
builder.Services.AddMudServices();

await builder.Build().RunAsync();
