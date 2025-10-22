using LoftComputacion.Application;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servicios básicos de la API ---
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Esta es la opción clave que rompe los ciclos
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


// --- 2. Nuestros servicios de la capa de Aplicación ---
builder.Services.AddScoped<OrdenDeServicioService>();
builder.Services.AddScoped<AIService>();
builder.Services.AddScoped<GananciasService>();
builder.Services.AddScoped<BlobService>();

// --- 3. Conexión a la base de datos (DbContext) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    }));

// --- Construimos la aplicación ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();