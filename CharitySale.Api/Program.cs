using CharitySale.Api.Context;
using CharitySale.Api.Repositories;
using CharitySale.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        //PostgreSQL
        builder.Services.AddDbContext<CharitySaleDbContext>(o =>
            o.UseNpgsql(builder.Configuration.GetConnectionString("CharitySaleDb")));

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        //Register repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped<IItemRepository, ItemRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        
        //Register services
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<ISaleService, SaleService>();
        
        // Register AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));


        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CharitySale API v1"));

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.MapOpenApi();
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        
        //Apply DB migrations and seed data
        await DatabaseInitializer.InitializeAsync(app.Services);

        await app.RunAsync();
    }
}