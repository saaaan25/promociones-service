using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using DotNetEnv;
using promociones.Data;
using promociones.Interfaces;
using promociones.Repository;
using promociones.Services;
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("DATABASE_URL environment variable is not set.");
}

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPromocionRepository, PromocionRepository>();

builder.Services.AddHttpClient<ProductoSyncService>();
builder.Services.AddScoped<ProductoSyncService>();

builder.Services.AddHostedService<promociones.Services.PeriodicSyncService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();