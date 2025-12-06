using LoftComputacion.Application;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Components;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===============================================
// 1) Configuración de Base de Datos
// ===============================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===============================================
// 2) Servicios de CORS (Permisivo para evitar errores)
// ===============================================
var MyAllowAllOrigins = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAllOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddHttpClient();

// ===============================================
// 3) Servicios de Aplicación (Inyección de Dependencias)
// ===============================================
builder.Services.AddScoped<OrdenDeServicioService>();
builder.Services.AddScoped<MercadoPagoService>();
builder.Services.AddSingleton<BlobService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AIService>();
builder.Services.AddScoped<EmailService>();

// ===============================================
// 4) JWT Authentication
// ===============================================
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ===============================================
// 5) Controllers y Componentes Web
// ===============================================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ===============================================
// 6) Swagger Config
// ===============================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoftComputacion API", Version = "v1" });

    // Configuración para el candadito de Authorization en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ===============================================
// 7) BUILD APP
// ===============================================
var app = builder.Build();

// ===============================================
// 8) PIPELINE (El orden aquí es SAGRADO)
// ===============================================

// A. Configuración de Swagger
// Quitamos el RoutePrefix para que NO se coma la página principal.
// Swagger estará disponible en /swagger/index.html
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loft API V1");
});

// B. Redirección y Archivos Estáticos
app.UseHttpsRedirection();

// IMPORTANTE: UseBlazorFrameworkFiles debe ir antes de UseStaticFiles
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// C. Enrutamiento (Faltaba en tu código anterior)
app.UseRouting();

// D. CORS (Debe ir entre UseRouting y UseAuth)
app.UseCors(MyAllowAllOrigins);

// E. Seguridad
app.UseAuthentication();
app.UseAuthorization();

// F. Mapeo de Endpoints
app.MapRazorPages();
app.MapControllers();

// G. Fallback (Lo más importante para Blazor)
// Si no encuentra ruta de API, devuelve la app de Blazor (index.html)
app.MapFallbackToFile("index.html");

app.Run();