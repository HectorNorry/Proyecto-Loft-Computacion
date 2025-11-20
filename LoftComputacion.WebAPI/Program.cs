using LoftComputacion.Application;
using LoftComputacion.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===============================================
// 1) CORS para Blazor
// ===============================================
var MyCors = "AllowBlazor";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCors,
        policy =>
        {
            policy.WithOrigins("https://localhost:7127")   // Blazor WASM
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ===============================================
// 2) DbContext
// ===============================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

// ===============================================
// 3) Servicios de aplicación
// ===============================================
builder.Services.AddScoped<OrdenDeServicioService>();
builder.Services.AddScoped<MercadoPagoService>();
builder.Services.AddSingleton<BlobService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<UsuarioService>();


// 👇 AÑADIR ESTOS SI NO ESTABAN REGISTRADOS
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
// 5) Controllers
// ===============================================
builder.Services.AddControllers();

// ===============================================
// 6) Swagger con soporte JWT
// ===============================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoftComputacion API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer. Ej: \"Bearer {token}\"",
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
// 7) Build
// ===============================================
var app = builder.Build();

// ===============================================
// 8) Pipeline
// ===============================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS antes de Auth
app.UseCors(MyCors);

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
