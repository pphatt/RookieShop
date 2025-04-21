using System;
using System.Data;

using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
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

        var createdBy = adminId;

        var brand1 = Brand.Create(
            name: "Sony",
            description: "Sony",
            createdBy: createdBy
        );

        var brand2 = Brand.Create(
            name: "Hifiman",
            description: "Hifiman",
            createdBy: createdBy
        );

        context.SaveChanges();

        #endregion Brand List
    }
}
