using LoftComputacion.Application;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servicios (CORS, JWT, DB, etc.) ---
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:52004", // Para tus pruebas locales
                "https://loftcomputacion-api-webapp.azurewebsites.net" // ⬅️ ¡La URL de tu API!
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    // ... (Tu configuración de Swagger) ...
});
builder.Services.AddScoped<OrdenDeServicioService>();
builder.Services.AddScoped<AIService>();
builder.Services.AddScoped<GananciasService>();
builder.Services.AddScoped<BlobService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<MercadoPagoService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    }));

// --- 2. Construcción de la app ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // 🚨 AÑADIR ESTA LÍNEA para el debug de Blazor
    app.UseWebAssemblyDebugging();
}

//app.UseHttpsRedirection();

// 🚨 PASO 3: CONFIGURACIÓN DE HOSTING DE BLAZOR 🚨
app.UseBlazorFrameworkFiles(); // <-- Sirve los archivos de Blazor
app.UseStaticFiles();

// --- Aplicar CORS y autenticación ---
app.UseCors("AllowBlazorApp");
app.UseAuthentication();
app.UseAuthorization();

// 🚨 Redirige todo lo que no sea API a Blazor
app.MapFallbackToFile("index.html");

app.MapControllers();
app.Run();