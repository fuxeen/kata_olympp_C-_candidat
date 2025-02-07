using Kata.Application.Services;
using Kata.Domain.Interfaces;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Kata.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<KataDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
}));

builder.Services.AddTransient<IClanRepository, ClanRepository>();
builder.Services.AddTransient<IBattleRepository, BattleRepository>();
builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IBattleService, BattleService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    try
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Kata API",
            Version = "v1"
        });

        c.UseOneOfForPolymorphism();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur pendant l'initialisation de Swagger : " + ex.Message);
        throw;
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kata API v1"));
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();