using FastEndpoints;
using FastEndpoints.Swagger;
using Playground.Infraestructure;
using Playground.WebApi;

/// <summary>
/// Clase principal para configurar y ejecutar la aplicación web. 
/// Configura servicios esenciales como infraestructura, esquema de seguridad, CORS, y documentación Swagger. 
/// También inicializa y ejecuta la aplicación, aplicando autenticación, autorización y configuraciones de endpoints.
/// </summary>
/// 

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddSecuritySchema(builder.Configuration);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // URL del frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddFastEndpoints()
                .SwaggerDocument()
                .AddHealthChecks();

var app = builder.Build();

// Aplicar CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(opt =>
{
    opt.Endpoints.RoutePrefix = "api";
});

if (app.Environment.IsDevelopment())
    app.UseSwaggerGen();

await app.Services.AddSeeds();

app.Run();