using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.DbContexts;
using E_Commerce.Persistence.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegisteration
    {

        public static async Task<WebApplication> MigrateIdentityDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();

            var dbContextService = Scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();

            var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
                await dbContextService.Database.MigrateAsync();

            return app;
        }
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();

            var dbContextService = Scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
                await dbContextService.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var DataInatializerService = Scope.ServiceProvider.GetRequiredKeyedService<IDataInatializer>("Default");
            await DataInatializerService.InatializeAsync();

            return app;
        }
        public static async Task<WebApplication> SeedIdentityDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var DataInatializerService = Scope.ServiceProvider.GetRequiredKeyedService<IDataInatializer>("Identity");
            await DataInatializerService.InatializeAsync();

            return app;
        }

    }
}
