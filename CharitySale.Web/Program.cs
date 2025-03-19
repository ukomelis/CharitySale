using CharitySale.Web.Components;
using CharitySale.Web.Services;

namespace CharitySale.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddHttpClient("CharitySaleApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:8080/"); //TODO: Move to configuration
        });
        
        // Add services
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<ISaleService, SaleService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}