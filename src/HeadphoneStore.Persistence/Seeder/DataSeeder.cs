using ErrorOr;

using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Persistence.DependencyInjection.Extensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Seeder;

public partial class DataSeeder
{
    public async static Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<AppRole> roleManager)
    {
        Guid adminId = Guid.NewGuid();

        // seed users.
        var passwordHasher = new PasswordHasher<AppUser>();

        // seed roles.
        var roles = await RoleList(context);

        // seed categories.
        SeedCategories(context, adminId);

        // seed brands.
        SeedBrands(context, adminId);

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

            admin.Id = adminId;

            await context.UserRoles.AddAsync(new IdentityUserRole<Guid> { RoleId = roles[0].Id, UserId = admin.Id, });

            await context.Users.AddAsync(admin);

            await context.SaveChangesAsync();
        }

        // seed permissions
        await SeedPermissions(context, roles);
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

    private static async Task SeedPermissions(ApplicationDbContext context, List<AppRole> roles)
    {
        #region Seed Permissions

        if (await context.Set<Permission>().AnyAsync())
        {
            return;
        }

        var adminRole = roles[0];
        var customerRole = roles[1];

        var allPermissions = new List<Permission>();

        foreach (var functionField in typeof(Permissions.Function).GetFields())
        {
            string function = functionField.GetValue(null)?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(function)) continue;

            foreach (var commandField in typeof(Permissions.Command).GetFields())
            {
                string command = commandField.GetValue(null)?.ToString() ?? string.Empty;

                if (string.IsNullOrEmpty(command)) continue;

                allPermissions.Add(new Permission(adminRole.Id, function.ToFunctionPermissions(), command.ToCommandPermissions()));
            }
        }

        allPermissions.Add(new Permission(customerRole.Id, Permissions.Function.CATEGORY.ToFunctionPermissions(), Permissions.Command.VIEW.ToCommandPermissions()));
        allPermissions.Add(new Permission(customerRole.Id, Permissions.Function.ORDER.ToFunctionPermissions(), Permissions.Command.VIEW.ToCommandPermissions()));
        allPermissions.Add(new Permission(customerRole.Id, Permissions.Function.ORDER.ToFunctionPermissions(), Permissions.Command.CREATE.ToCommandPermissions()));

        await context.Permissions.AddRangeAsync(allPermissions);

        await context.SaveChangesAsync();

        #endregion Seed Permissions
    }

    private static void SeedCategories(ApplicationDbContext context, Guid adminId)
    {
        #region Category List

        if (context.Categories.Any())
        {
            return;
        }

        var createdBy = adminId;

        var taiNghe = Category.Create(
            name: "Tai Nghe",
            description: "Danh mục các loại tai nghe chất lượng cao.",
            createdBy: createdBy
        );

        var taiNgheSubCategories = new[]
        {
            Category.Create("Tai Nghe In Ear", "Tai nghe nhét tai nhỏ gọn, tiện lợi.", createdBy, taiNghe),
            Category.Create("Tai Nghe Over Ear", "Tai nghe trùm tai với chất lượng âm thanh vượt trội.", createdBy, taiNghe),
            Category.Create("Tai Nghe Phòng Thu", "Tai nghe chuyên dụng cho phòng thu âm.", createdBy, taiNghe),
            Category.Create("Tai Nghe Chống Ồn", "Tai nghe với công nghệ chống ồn chủ động.", createdBy, taiNghe),
            Category.Create("Tai Nghe Bluetooth", "Tai nghe không dây sử dụng công nghệ Bluetooth.", createdBy, taiNghe),
            Category.Create("Tai Nghe True Wireless", "Tai nghe không dây hoàn toàn, nhỏ gọn và hiện đại.", createdBy, taiNghe)
        };

        foreach (var subCategory in taiNgheSubCategories)
        {
            taiNghe.AddSubCategory(subCategory);
        }

        var dacAmp = Category.Create(
            name: "DAC - AMP",
            description: "Danh mục các thiết bị DAC và AMP cải thiện chất lượng âm thanh.",
            createdBy: createdBy
        );

        var dacAmpSubCategories = new[]
        {
            Category.Create("DAC", "Thiết bị chuyển đổi tín hiệu số sang analog.", createdBy, dacAmp),
            Category.Create("DAC/AMP Di Động", "Thiết bị DAC và AMP nhỏ gọn, phù hợp di chuyển.", createdBy, dacAmp)
        };

        foreach (var subCategory in dacAmpSubCategories)
        {
            dacAmp.AddSubCategory(subCategory);
        }

        context.Categories.AddRange(taiNghe, dacAmp);

        context.SaveChanges();

        #endregion Category List
    }

    private static void SeedBrands(ApplicationDbContext context, Guid adminId)
    {
        #region Brand List

        if (context.Brands.Any())
        {
            return;
        }

        var brands = new List<Brand>
        {
            Brand.Create(name: "Sony", description: "Sony", createdBy: adminId),
            Brand.Create(name: "64 Audio", description: "64 Audio", createdBy: adminId),
            Brand.Create(name: "AAW", description: "AAW", createdBy: adminId),
            Brand.Create(name: "Apple", description: "Apple", createdBy: adminId),
            Brand.Create(name: "Fiil", description: "Fiil", createdBy: adminId),
            Brand.Create(name: "JBL", description: "JBL", createdBy: adminId),
            Brand.Create(name: "Audio-technica", description: "Audio-technica", createdBy: adminId),
            Brand.Create(name: "Auglamour", description: "Auglamour", createdBy: adminId),
            Brand.Create(name: "Skullcandy", description: "Skullcandy", createdBy: adminId),
            Brand.Create(name: "SoundPeats", description: "SoundPeats", createdBy: adminId),
            Brand.Create(name: "Beats", description: "Beats", createdBy: adminId),
            Brand.Create(name: "Beyerdynamic", description: "Beyerdynamic", createdBy: adminId),
            Brand.Create(name: "B&O", description: "B&O", createdBy: adminId),
            Brand.Create(name: "Bose", description: "Bose", createdBy: adminId),
            Brand.Create(name: "Campfire", description: "Campfire", createdBy: adminId),
            Brand.Create(name: "Focal", description: "Focal", createdBy: adminId),
            Brand.Create(name: "Denon", description: "Denon", createdBy: adminId),
            Brand.Create(name: "Grado", description: "Grado", createdBy: adminId),
            Brand.Create(name: "Hifiman", description: "Hifiman", createdBy: adminId),
            Brand.Create(name: "Jabra", description: "Jabra", createdBy: adminId),
            Brand.Create(name: "Sennheiser", description: "Sennheiser", createdBy: adminId),
            Brand.Create(name: "Shozy", description: "Shozy", createdBy: adminId),
            Brand.Create(name: "Shure", description: "Shure", createdBy: adminId),
            Brand.Create(name: "Tribit", description: "Tribit", createdBy: adminId),
            Brand.Create(name: "SoundMAGIC", description: "SoundMAGIC", createdBy: adminId),
            Brand.Create(name: "iBasso", description: "iBasso", createdBy: adminId),
            Brand.Create(name: "Fiio", description: "Fiio", createdBy: adminId),
            Brand.Create(name: "Sabbat", description: "Sabbat", createdBy: adminId),
            Brand.Create(name: "Moondrop", description: "Moondrop", createdBy: adminId),
            Brand.Create(name: "Marshall", description: "Marshall", createdBy: adminId),
            Brand.Create(name: "Westone", description: "Westone", createdBy: adminId),
            Brand.Create(name: "Dunu", description: "Dunu", createdBy: adminId),
            Brand.Create(name: "Yuin", description: "Yuin", createdBy: adminId),
            Brand.Create(name: "Campire", description: "Campire", createdBy: adminId)
        };

        context.Brands.AddRange(brands);

        context.SaveChanges();

        #endregion Brand List
    }
}
