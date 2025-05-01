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
            slug: "tai-nghe",
            description: "Danh mục các loại tai nghe chất lượng cao.",
            createdBy: createdBy
        );

        var taiNgheSubCategories = new[]
        {
            Category.Create("Tai Nghe In Ear", "tai-nghe-in-ear", "Tai nghe nhét tai nhỏ gọn, tiện lợi.", createdBy, taiNghe),
            Category.Create("Tai Nghe Over Ear", "tai-nghe-over-ear", "Tai nghe trùm tai với chất lượng âm thanh vượt trội.", createdBy, taiNghe),
            Category.Create("Tai Nghe Phòng Thu", "tai-nghe-phong-thu", "Tai nghe chuyên dụng cho phòng thu âm.", createdBy, taiNghe),
            Category.Create("Tai Nghe Chống Ồn", "tai-nghe-chong-on", "Tai nghe với công nghệ chống ồn chủ động.", createdBy, taiNghe),
            Category.Create("Tai Nghe Bluetooth", "tai-nghe-bluetooth", "Tai nghe không dây sử dụng công nghệ Bluetooth.", createdBy, taiNghe),
            Category.Create("Tai Nghe True Wireless", "tai-nghe-true-wireless", "Tai nghe không dây hoàn toàn, nhỏ gọn và hiện đại.", createdBy, taiNghe)
        };

        foreach (var subCategory in taiNgheSubCategories)
        {
            taiNghe.AddSubCategory(subCategory);
        }

        var dacAmp = Category.Create(
            name: "DACs & Amplifiers",
            slug: "dac-amp",
            description: "Danh mục các thiết bị DAC và Amplifier cải thiện chất lượng âm thanh.",
            createdBy: createdBy
        );

        var dacAmpSubCategories = new[]
        {
            Category.Create("DAC", "dac-may-tinh", "Thiết bị chuyển đổi tín hiệu số sang analog.", createdBy, dacAmp),
            Category.Create("DAC/AMP Di Động", "dac-di-dong", "Thiết bị DAC và AMP nhỏ gọn, phù hợp di chuyển.", createdBy, dacAmp)
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
            Brand.Create(name: "Sony", slug: "sony", description: "Sony", createdBy: adminId),
            Brand.Create(name: "64 Audio", slug: "64-audio", description: "64 Audio", createdBy: adminId),
            Brand.Create(name: "AAW", slug: "aaw", description: "AAW", createdBy: adminId),
            Brand.Create(name: "Apple", slug: "apple", description: "Apple", createdBy: adminId),
            Brand.Create(name: "Fiil", slug: "fiil", description: "Fiil", createdBy: adminId),
            Brand.Create(name: "JBL", slug: "jbl", description: "JBL", createdBy: adminId),
            Brand.Create(name: "Audio-technica", slug: "audio-technica", description: "Audio-technica", createdBy: adminId),
            Brand.Create(name: "Auglamour", slug: "auglamour", description: "Auglamour", createdBy: adminId),
            Brand.Create(name: "Skullcandy", slug: "skullcandy", description: "Skullcandy", createdBy: adminId),
            Brand.Create(name: "SoundPeats", slug: "soundpeats", description: "SoundPeats", createdBy: adminId),
            Brand.Create(name: "Beats", slug: "beats", description: "Beats", createdBy: adminId),
            Brand.Create(name: "Beyerdynamic", slug: "beyerdynamic", description: "Beyerdynamic", createdBy: adminId),
            Brand.Create(name: "B&O", slug: "bo", description: "B&O", createdBy: adminId),
            Brand.Create(name: "Bose", slug: "bose", description: "Bose", createdBy: adminId),
            Brand.Create(name: "Campfire", slug: "campfire", description: "Campfire", createdBy: adminId),
            Brand.Create(name: "Focal", slug: "focal", description: "Focal", createdBy: adminId),
            Brand.Create(name: "Denon", slug: "denon", description: "Denon", createdBy: adminId),
            Brand.Create(name: "Grado", slug: "grado", description: "Grado", createdBy: adminId),
            Brand.Create(name: "Hifiman", slug: "hifiman", description: "Hifiman", createdBy: adminId),
            Brand.Create(name: "Jabra", slug: "jabra", description: "Jabra", createdBy: adminId),
            Brand.Create(name: "Sennheiser", slug: "sennheiser", description: "Sennheiser", createdBy: adminId),
            Brand.Create(name: "Shozy", slug: "shozy", description: "Shozy", createdBy: adminId),
            Brand.Create(name: "Shure", slug: "shure", description: "Shure", createdBy: adminId),
            Brand.Create(name: "Tribit", slug: "tribit", description: "Tribit", createdBy: adminId),
            Brand.Create(name: "SoundMAGIC", slug: "soundmagic", description: "SoundMAGIC", createdBy: adminId),
            Brand.Create(name: "iBasso", slug: "ibasso", description: "iBasso", createdBy: adminId),
            Brand.Create(name: "Fiio", slug: "fiio", description: "Fiio", createdBy: adminId),
            Brand.Create(name: "Sabbat", slug: "sabbat", description: "Sabbat", createdBy: adminId),
            Brand.Create(name: "Moondrop", slug: "moondrop", description: "Moondrop", createdBy: adminId),
            Brand.Create(name: "Marshall", slug: "marshall", description: "Marshall", createdBy: adminId),
            Brand.Create(name: "Westone", slug: "westone", description: "Westone", createdBy: adminId),
            Brand.Create(name: "Dunu", slug: "dunu", description: "Dunu", createdBy: adminId),
            Brand.Create(name: "Yuin", slug: "yuin", description: "Yuin", createdBy: adminId),
            Brand.Create(name: "Campire", slug: "campire", description: "Campire", createdBy: adminId)
        };

        context.Brands.AddRange(brands);

        context.SaveChanges();

        #endregion Brand List
    }
}
