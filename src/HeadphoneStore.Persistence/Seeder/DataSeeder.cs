using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Seeder;

public partial class DataSeeder
{
    public async static Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<AppRole> roleManager)
    {
        var adminId = Guid.NewGuid();

        // seed users.
        var passwordHasher = new PasswordHasher<AppUser>();

        var managerId = Guid.NewGuid();

        if (!await context.Users.AnyAsync())
        {
            // create admin account.
            var email = "phatvtgcs21@gmail.com";
            var username = "admin";

            var admin = new AppUser()
            {
                Id = adminId,
                FirstName = "Tien Phat",
                LastName = "Vu",
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                // không có SecurityStamp, user không thể login được.
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                IsActive = true,
                CreatedOnUtc = DateTimeOffset.UtcNow,
            };

            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");

            await context.Users.AddAsync(admin);

            await context.SaveChangesAsync();
        }
    }
}
