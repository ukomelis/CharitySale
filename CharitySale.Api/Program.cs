using System.Reflection;
using CharitySale.Api.Context;
using CharitySale.Api.Repositories;
using CharitySale.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CharitySale.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddDbContext<CharitySaleDbContext>(o =>
            o.UseNpgsql(builder.Configuration.GetConnectionString("CharitySaleDb")));

        builder.Services.AddControllers();
        
        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", b =>
            {
                b.WithOrigins(builder.Configuration.GetSection("ClientApp:BaseUrl").Value!)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CharitySale API",
                Version = "v1",
                Description = "API for managing charity sale items and processing sales"
            });
    
            // Enable XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        
        //Register repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped<IItemRepository, ItemRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        //Register services
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<ISaleService, SaleService>();
        
        // Register AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        
        builder.Services.AddSignalR();

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CharitySale API v1"));

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseCors("AllowReactApp");
        app.MapHub<CharitySaleHub>("/charitySaleHub");
        
        //Apply DB migrations and seed data
        await DatabaseInitializer.InitializeAsync(app.Services);

        await app.RunAsync();
    }
}