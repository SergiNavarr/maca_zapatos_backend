using API.Services;
using Application.Interfaces;
using Application.Services;
using Infraestructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// 1. Registrar nuestro usuario falso temporal para que la auditoría funcione
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// 2. Inyectar toda la Infraestructura (DbContext, Interceptores, Postgres)
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddScoped<IProductoService, ProductoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();