using System;
using System.Data;

using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Seeder;

public partial class DataSeeder
{
    public async static Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<AppRole> roleManager)
    {
        Guid adminId;

        // seed users.
        var passwordHasher = new PasswordHasher<AppUser>();

        // seed roles.
        var roles = await RoleList(context);

        if (!await context.Users.AnyAsync())
        {
            // create admin account.
            var email = "phatvtgcs21@gmail.com";
            var username = "admin";

            var admin = AppUser.Create(
                email: email,
                firstName: "Tien Phat",
                lastName: "Vu",
                phoneNumber: "0123456789"
            );

            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");

            adminId = admin.Id;

            await context.UserRoles.AddAsync(new IdentityUserRole<Guid> { RoleId = roles[0].Id, UserId = admin.Id, });

            await context.Users.AddAsync(admin);

            await context.SaveChangesAsync();
        }
    }

    private static async Task<List<AppRole>> RoleList(ApplicationDbContext context)
    {
        #region Role List

        var roles = new List<AppRole>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpperInvariant(),
                Description = "Administrator Role",
                DisplayName = "Administrator",
                Status = RoleStatus.Active
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = Roles.Customer,
                NormalizedName = Roles.Customer.ToUpperInvariant(),
                Description = "Customer Role",
                DisplayName = "Customer",
                Status = RoleStatus.Active
            }
        };

        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        return roles;

        #endregion Role List
    }
}
