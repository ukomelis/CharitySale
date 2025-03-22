using CharitySale.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, int maxRetries = 5)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CharitySaleDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        await WaitForDatabaseAsync(context, logger, maxRetries);
        await ApplyMigrationsAsync(context, logger);
    }

    private static async Task WaitForDatabaseAsync(CharitySaleDbContext context, ILogger logger, int maxRetries)
    {
        for (var i = 0; i < maxRetries; i++)
        {
            try
            {
                logger.LogInformation("Attempting to connect to the database");
                if (await context.Database.CanConnectAsync())
                    return;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Database connection attempt {Attempt} of {MaxRetries} failed", i + 1, maxRetries);
            }

            if (i < maxRetries - 1)
                await Task.Delay(TimeSpan.FromSeconds(5));
        }
        
        throw new Exception("Could not connect to the database after multiple attempts");
    }

    private static async Task ApplyMigrationsAsync(CharitySaleDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations and seeding applied successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations and seeding data");
            throw;
        }
    }
}