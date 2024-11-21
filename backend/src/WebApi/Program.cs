using FastEndpoints;
using FastEndpoints.Swagger;
using Playground.Infraestructure;
using Playground.WebApi;

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

await app.Services.SeedRoles();

app.Run();