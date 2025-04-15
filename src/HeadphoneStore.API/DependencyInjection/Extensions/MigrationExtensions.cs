using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Persistence;
using HeadphoneStore.Persistence.Seeder;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.API.DependencyInjection.Extensions;

public static class MigrationExtensions
{
    public static WebApplication AddMigration(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        using var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        // apply update-database command here.
        appDbContext.Database.Migrate();
        DataSeeder.SeedAsync(appDbContext, roleManager).GetAwaiter().GetResult();

        return app;
    }
}
