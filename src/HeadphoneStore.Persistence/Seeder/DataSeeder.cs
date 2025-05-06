using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;
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
        var categories = SeedCategories(context, adminId);

        // seed brands.
        var brands = SeedBrands(context, adminId);

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

        // seed products
        SeedProducts(context, brands, categories, adminId);
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

                allPermissions.Add(new Permission(adminRole.Id, function.ToFunctionPermissions(),
                    command.ToCommandPermissions()));
            }
        }

        allPermissions.Add(new Permission(customerRole.Id, Permissions.Function.CATEGORY.ToFunctionPermissions(),
            Permissions.Command.VIEW.ToCommandPermissions()));
        allPermissions.Add(new Permission(customerRole.Id, Permissions.Function.ORDER.ToFunctionPermissions(),
            Permissions.Command.VIEW.ToCommandPermissions()));
        allPermissions.Add(new Permission(customerRole.Id, Permissions.Function.ORDER.ToFunctionPermissions(),
            Permissions.Command.CREATE.ToCommandPermissions()));

        await context.Permissions.AddRangeAsync(allPermissions);

        await context.SaveChangesAsync();

        #endregion Seed Permissions
    }

    private static List<Category> SeedCategories(ApplicationDbContext context, Guid adminId)
    {
        #region Category List

        if (context.Categories.Any())
        {
            return [];
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
            Category.Create("Tai Nghe In Ear", "tai-nghe-in-ear", "Tai nghe nhét tai nhỏ gọn, tiện lợi.", createdBy,
                taiNghe),
            Category.Create("Tai Nghe Over Ear", "tai-nghe-over-ear",
                "Tai nghe trùm tai với chất lượng âm thanh vượt trội.", createdBy, taiNghe),
            Category.Create("Tai Nghe Phòng Thu", "tai-nghe-phong-thu", "Tai nghe chuyên dụng cho phòng thu âm.",
                createdBy, taiNghe),
            Category.Create("Tai Nghe Chống Ồn", "tai-nghe-chong-on", "Tai nghe với công nghệ chống ồn chủ động.",
                createdBy, taiNghe),
            Category.Create("Tai Nghe Bluetooth", "tai-nghe-bluetooth",
                "Tai nghe không dây sử dụng công nghệ Bluetooth.", createdBy, taiNghe),
            Category.Create("Tai Nghe True Wireless", "tai-nghe-true-wireless",
                "Tai nghe không dây hoàn toàn, nhỏ gọn và hiện đại.", createdBy, taiNghe)
        };

        foreach (var subCategory in taiNgheSubCategories)
        {
            taiNghe.AddSubCategory(subCategory);
        }

        var dacAmp = Category.Create(
            name: "DACs & Amplifiers",
            slug: "dac",
            description: "Danh mục các thiết bị DAC và Amplifier cải thiện chất lượng âm thanh.",
            createdBy: createdBy
        );

        var dacAmpSubCategories = new[]
        {
            Category.Create("DAC", "dac-may-tinh", "Thiết bị chuyển đổi tín hiệu số sang analog.", createdBy,
                dacAmp),
            Category.Create("DAC/AMP Di Động", "dac-di-dong", "Thiết bị DAC và AMP nhỏ gọn, phù hợp di chuyển.",
                createdBy, dacAmp)
        };

        foreach (var subCategory in dacAmpSubCategories)
        {
            dacAmp.AddSubCategory(subCategory);
        }

        context.Categories.AddRange(taiNghe, dacAmp);

        context.SaveChanges();

        return [..taiNgheSubCategories, ..dacAmpSubCategories];

        #endregion Category List
    }

    private static List<Brand> SeedBrands(ApplicationDbContext context, Guid adminId)
    {
        #region Brand List

        if (context.Brands.Any())
        {
            return [];
        }

        var brands = new List<Brand>
        {
            Brand.Create(name: "Sony", slug: "sony", description: "Sony", createdBy: adminId),
            Brand.Create(name: "64 Audio", slug: "64-audio", description: "64 Audio", createdBy: adminId),
            Brand.Create(name: "AAW", slug: "aaw", description: "AAW", createdBy: adminId),
            Brand.Create(name: "Apple", slug: "apple", description: "Apple", createdBy: adminId),
            Brand.Create(name: "Fiil", slug: "fiil", description: "Fiil", createdBy: adminId),
            Brand.Create(name: "JBL", slug: "jbl", description: "JBL", createdBy: adminId),
            Brand.Create(name: "Audio-technica", slug: "audio-technica", description: "Audio-technica",
                createdBy: adminId),
            Brand.Create(name: "Auglamour", slug: "auglamour", description: "Auglamour", createdBy: adminId),
            Brand.Create(name: "Skullcandy", slug: "skullcandy", description: "Skullcandy", createdBy: adminId),
            Brand.Create(name: "SoundPeats", slug: "soundpeats", description: "SoundPeats", createdBy: adminId),
            Brand.Create(name: "Beats", slug: "beats", description: "Beats", createdBy: adminId),
            Brand.Create(name: "Beyerdynamic", slug: "beyerdynamic", description: "Beyerdynamic",
                createdBy: adminId),
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
            Brand.Create(name: "Campire", slug: "campire", description: "Campire", createdBy: adminId),
            Brand.Create(name: "Topping", slug: "topping", description: "Topping", createdBy: adminId),
            Brand.Create(name: "SMSL", slug: "smsl", description: "SMSL", createdBy: adminId),
        };

        context.Brands.AddRange(brands);

        context.SaveChanges();

        return brands;

        #endregion Brand List
    }

    private static void SeedProducts(ApplicationDbContext context, List<Brand> brands, List<Category> categories,
        Guid adminId)
    {
        #region Product List

        if (context.Products.Any())
        {
            return;
        }

        var products = new List<Product>
        {
            new Product(
                name: "Tai nghe Moondrop Psyche",
                description: "Tai nghe Moondrop Psyche",
                productStatus: ProductStatus.InStock,
                productPrice: ProductPrice.Create(48490000),
                stock: 16,
                sold: 0,
                sku: "tai-nghe-moondrop-psyche",
                slug: "tai-nghe-moondrop-psyche",
                category: categories[1], // IEM
                brand: brands[28], // Moondrop
                status: EntityStatus.Inactive,
                createdBy: adminId
            ),
            new Product(
                name: "Topping PA5 II Compact Desktop Amplifier",
                description: "Topping PA5 II Compact Desktop Amplifier",
                productStatus: ProductStatus.InStock,
                productPrice: ProductPrice.Create(4990000),
                stock: 19,
                sold: 0,
                sku: "topping-pa5-ii-compact-desktop-amplifier",
                slug: "topping-pa5-ii-compact-desktop-amplifier",
                category: categories[6], // Amplifier
                brand: brands[33], // Topping
                status: EntityStatus.Inactive,
                createdBy: adminId
            ),
            new Product(
                name: "SMSL PA40 Digital Power Amplifier",
                description: "SMSL PA40 Digital Power Amplifier",
                productStatus: ProductStatus.InStock,
                productPrice: ProductPrice.Create(3590000),
                stock: 10,
                sold: 0,
                sku: "smsl-pa40-digital-power-amplifier",
                slug: "smsl-pa40-digital-power-amplifier",
                category: categories[7], // Amplifier
                brand: brands[34], // SMSL
                status: EntityStatus.Inactive,
                createdBy: adminId
            ),
            new Product(
                name: "Tai nghe Audio Technica ATH-M70X",
                description: "Tai nghe Audio Technica ATH-M70X",
                productStatus: ProductStatus.InStock,
                productPrice: ProductPrice.Create(5990000),
                stock: 13,
                sold: 0,
                sku: "tai-nghe-audio-technica-ath-m70x",
                slug: "tai-nghe-audio-technica-ath-m70x",
                category: categories[3], // Over-ear
                brand: brands[6], // Audio-Technica
                status: EntityStatus.Inactive,
                createdBy: adminId
            ),
            new Product(
                name: "Tai nghe HiFiMan Sundara",
                description: "Tai nghe HiFiMan Sundara",
                productStatus: ProductStatus.InStock,
                productPrice: ProductPrice.Create(6400000),
                stock: 20,
                sold: 0,
                sku: "tai-nghe-hifiman-sundara",
                slug: "tai-nghe-hifiman-sundara",
                category: categories[2], // Open-back
                brand: brands[18], // Hifiman
                status: EntityStatus.Inactive,
                createdBy: adminId
            ),
            //new Product(
            //    name: "Tai nghe True Wireless Moondrop Robin",
            //    description: "Tai nghe True Wireless Moondrop Robin",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(2690000),
            //    stock: 14,
            //    sold: 0,
            //    sku: "tai-nghe-true-wireless-moondrop-robin",
            //    slug: "tai-nghe-true-wireless-moondrop-robin",
            //    category: categories[6], // True Wireless
            //    brand: brands[30], // Moondrop
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe Dunu Kima 2",
            //    description: "Tai nghe Dunu Kima 2",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(2990000),
            //    stock: 17,
            //    sold: 0,
            //    sku: "tai-nghe-dunu-kima-2",
            //    slug: "tai-nghe-dunu-kima-2",
            //    category: categories[1], // IEM
            //    brand: brands[5], // Dunu
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe Sennheiser Momentum True Wireless 4",
            //    description: "Tai nghe Sennheiser Momentum True Wireless 4",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(8690000),
            //    stock: 11,
            //    sold: 0,
            //    sku: "tai-nghe-sennheiser-momentum-true-wireless-4",
            //    slug: "tai-nghe-sennheiser-momentum-true-wireless-4",
            //    category: categories[6], // True Wireless
            //    brand: brands[8], // Sennheiser
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe 7Hz Timeless II",
            //    description: "Tai nghe 7Hz Timeless II",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(5890000),
            //    stock: 18,
            //    sold: 0,
            //    sku: "tai-nghe-7hz-timeless-ii",
            //    slug: "tai-nghe-7hz-timeless-ii",
            //    category: categories[1], // IEM
            //    brand: brands[11], // 7Hz
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe Dunu DaVinci",
            //    description: "Tai nghe Dunu DaVinci",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(7490000),
            //    stock: 15,
            //    sold: 0,
            //    sku: "tai-nghe-dunu-davinci",
            //    slug: "tai-nghe-dunu-davinci",
            //    category: categories[1], // IEM
            //    brand: brands[5], // Dunu
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe True Wireless Moondrop Ultrasonic",
            //    description: "Tai nghe True Wireless Moondrop Ultrasonic",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(1890000),
            //    stock: 12,
            //    sold: 0,
            //    sku: "tai-nghe-true-wireless-moondrop-ultrasonic",
            //    slug: "tai-nghe-true-wireless-moondrop-ultrasonic",
            //    category: categories[6], // True Wireless
            //    brand: brands[12], // Moondrop
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe Sennheiser IE300",
            //    description: "Tai nghe Sennheiser IE300",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(8399000),
            //    stock: 19,
            //    sold: 0,
            //    sku: "tai-nghe-sennheiser-ie300",
            //    slug: "tai-nghe-sennheiser-ie300",
            //    category: categories[1], // IEM
            //    brand: brands[8], // Sennheiser
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe Truthear x Crinacle Zero:Blue2 - không Mic",
            //    description: "Tai nghe Truthear x Crinacle Zero:Blue2 - không Mic",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(1690000),
            //    stock: 13,
            //    sold: 0,
            //    sku: "tai-nghe-truthear-x-crinacle-zero-blue2-khong-mic",
            //    slug: "tai-nghe-truthear-x-crinacle-zero-blue2-khong-mic",
            //    category: categories[1], // IEM
            //    brand: brands[14], // Truthear
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe ThieAudio Oracle MKIII",
            //    description: "Tai nghe ThieAudio Oracle MKIII",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(14590000),
            //    stock: 11,
            //    sold: 0,
            //    sku: "tai-nghe-thieaudio-oracle-mkiii",
            //    slug: "tai-nghe-thieaudio-oracle-mkiii",
            //    category: categories[1], // IEM
            //    brand: brands[15], // ThieAudio
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //),
            //new Product(
            //    name: "Tai nghe Simgot EA500 LM",
            //    description: "Tai nghe Simgot EA500 LM",
            //    productStatus: ProductStatus.InStock,
            //    productPrice: ProductPrice.Create(2190000),
            //    stock: 18,
            //    sold: 0,
            //    sku: "tai-nghe-simgot-ea500-lm",
            //    slug: "tai-nghe-simgot-ea500-lm",
            //    category: categories[1], // IEM
            //    brand: brands[18], // Simgot
            //    status: EntityStatus.Inactive,
            //    createdBy: adminId
            //)
        };

        var productMedia = new List<ProductMedia>
        {
            new ProductMedia(
                productId: products[0].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195298/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/uix6rr5upmrna409gnr6.jpg",
                publicId: "products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/uix6rr5upmrna409gnr6",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195298/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/uix6rr5upmrna409gnr6.jpg",
                name: "uix6rr5upmrna409gnr6",
                order: 1,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[0].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195299/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/l8liwzkufkpcmlcpa96b.webp",
                publicId: "products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/l8liwzkufkpcmlcpa96b",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195299/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/l8liwzkufkpcmlcpa96b.webp",
                name: "l8liwzkufkpcmlcpa96b",
                order: 2,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[0].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195300/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/t1behqnquuqvr23sgf40.webp",
                publicId: "products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/t1behqnquuqvr23sgf40",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195300/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/t1behqnquuqvr23sgf40.webp",
                name: "t1behqnquuqvr23sgf40",
                order: 3,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[0].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195300/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/kvyfhmwuotjauavrtkpa.webp",
                publicId: "products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/kvyfhmwuotjauavrtkpa",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195300/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/kvyfhmwuotjauavrtkpa.webp",
                name: "kvyfhmwuotjauavrtkpa",
                order: 4,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[0].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195301/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/hcqvihokv6bqwcuors0z.webp",
                publicId: "products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/hcqvihokv6bqwcuors0z",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746195301/products/e2614c75-4cf7-4a73-9cfa-510e89ffaab7/images/hcqvihokv6bqwcuors0z.webp",
                name: "hcqvihokv6bqwcuors0z",
                order: 5,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[1].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196620/products/e26450c8-4664-4be3-be54-71475921d629/images/zehlb5youebfg0bjklrv.jpg",
                publicId: "products/e26450c8-4664-4be3-be54-71475921d629/images/zehlb5youebfg0bjklrv",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196620/products/e26450c8-4664-4be3-be54-71475921d629/images/zehlb5youebfg0bjklrv.jpg",
                name: "zehlb5youebfg0bjklrv",
                order: 1,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[1].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196621/products/e26450c8-4664-4be3-be54-71475921d629/images/p1p2dwccir5kfqlx2le4.jpg",
                publicId: "products/e26450c8-4664-4be3-be54-71475921d629/images/p1p2dwccir5kfqlx2le4",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196621/products/e26450c8-4664-4be3-be54-71475921d629/images/p1p2dwccir5kfqlx2le4.jpg",
                name: "p1p2dwccir5kfqlx2le4",
                order: 2,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[1].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196622/products/e26450c8-4664-4be3-be54-71475921d629/images/gc0yl4a3lam3nlmp2qhk.jpg",
                publicId: "products/e26450c8-4664-4be3-be54-71475921d629/images/gc0yl4a3lam3nlmp2qhk",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196622/products/e26450c8-4664-4be3-be54-71475921d629/images/gc0yl4a3lam3nlmp2qhk.jpg",
                name: "gc0yl4a3lam3nlmp2qhk",
                order: 3,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[1].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196623/products/e26450c8-4664-4be3-be54-71475921d629/images/teqvaxz3ttk7xyvjqrdc.jpg",
                publicId: "products/e26450c8-4664-4be3-be54-71475921d629/images/teqvaxz3ttk7xyvjqrdc",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746196623/products/e26450c8-4664-4be3-be54-71475921d629/images/teqvaxz3ttk7xyvjqrdc.jpg",
                name: "teqvaxz3ttk7xyvjqrdc",
                order: 4,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497704/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/ag9nyap81oc1qbyvqdgr.jpg",
                publicId: "products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/ag9nyap81oc1qbyvqdgr",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497704/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/ag9nyap81oc1qbyvqdgr.jpg",
                name: "ag9nyap81oc1qbyvqdgr",
                order: 1,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497705/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/rsyy0jiv9oplzbwfggid.jpg",
                publicId: "products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/rsyy0jiv9oplzbwfggid",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497705/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/rsyy0jiv9oplzbwfggid.jpg",
                name: "rsyy0jiv9oplzbwfggid",
                order: 2,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497706/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/xrod3w3j5lnrlmfl5p7c.jpg",
                publicId: "products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/xrod3w3j5lnrlmfl5p7c",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497706/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/xrod3w3j5lnrlmfl5p7c.jpg",
                name: "xnmjksmrax8gm85rsum0",
                order: 3,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497706/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/snfldvybqzdqyf3lrsgl.jpg",
                publicId: "products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/snfldvybqzdqyf3lrsgl",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746497706/products/21bfd44a-1105-4c79-bc7e-1cf2c9c1b461/images/snfldvybqzdqyf3lrsgl.jpg",
                name: "snfldvybqzdqyf3lrsgl",
                order: 4,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746197914/products/289c691c-60ab-4f79-881f-a79dc1cae032/images/hmctd5d3ngu1qpkexzcy.jpg",
                publicId: "products/289c691c-60ab-4f79-881f-a79dc1cae032/images/hmctd5d3ngu1qpkexzcy",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746197914/products/289c691c-60ab-4f79-881f-a79dc1cae032/images/hmctd5d3ngu1qpkexzcy.jpg",
                name: "hmctd5d3ngu1qpkexzcy",
                order: 6,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746197914/products/289c691c-60ab-4f79-881f-a79dc1cae032/images/yhfyw4cqk7pbt170n6j0.jpg",
                publicId: "products/289c691c-60ab-4f79-881f-a79dc1cae032/images/yhfyw4cqk7pbt170n6j0",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746197914/products/289c691c-60ab-4f79-881f-a79dc1cae032/images/yhfyw4cqk7pbt170n6j0.jpg",
                name: "yhfyw4cqk7pbt170n6j0",
                order: 7,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[2].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746197916/products/289c691c-60ab-4f79-881f-a79dc1cae032/images/k84kdwtsuqqivxeovpcm.jpg",
                publicId: "products/289c691c-60ab-4f79-881f-a79dc1cae032/images/k84kdwtsuqqivxeovpcm",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746197916/products/289c691c-60ab-4f79-881f-a79dc1cae032/images/k84kdwtsuqqivxeovpcm.jpg",
                name: "k84kdwtsuqqivxeovpcm",
                order: 8,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[3].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192271/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/a03pjflticrqqd9h7dgj.jpg",
                publicId: "products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/a03pjflticrqqd9h7dgj",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192271/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/a03pjflticrqqd9h7dgj.jpg",
                name: "a03pjflticrqqd9h7dgj",
                order: 1,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[3].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192272/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/bndcqkqgcauewfgkkepz.jpg",
                publicId: "products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/bndcqkqgcauewfgkkepz",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192272/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/bndcqkqgcauewfgkkepz.jpg",
                name: "bndcqkqgcauewfgkkepz",
                order: 2,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[3].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192272/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/gjiuwbg9nphrelwniaik.jpg",
                publicId: "products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/gjiuwbg9nphrelwniaik",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192272/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/gjiuwbg9nphrelwniaik.jpg",
                name: "gjiuwbg9nphrelwniaik",
                order: 3,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[3].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192273/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/ffsmazqwxabdftzhjnax.jpg",
                publicId: "products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/ffsmazqwxabdftzhjnax",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192273/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/ffsmazqwxabdftzhjnax.jpg",
                name: "ffsmazqwxabdftzhjnax",
                order: 4,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[3].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192274/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/rf7fiyd5oxmfzjezkswt.jpg",
                publicId: "products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/rf7fiyd5oxmfzjezkswt",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746192274/products/ff894a93-38c6-44a5-956a-c323ba54c18e/images/rf7fiyd5oxmfzjezkswt.jpg",
                name: "rf7fiyd5oxmfzjezkswt",
                order: 5,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[4].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191741/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/w8vutibzgvh6ec3axwmm.jpg",
                publicId: "products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/w8vutibzgvh6ec3axwmm",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191741/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/w8vutibzgvh6ec3axwmm.jpg",
                name: "w8vutibzgvh6ec3axwmm",
                order: 1,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[4].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191743/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/mxrvpz5f88usxsknazc1.jpg",
                publicId: "products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/mxrvpz5f88usxsknazc1",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191743/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/mxrvpz5f88usxsknazc1.jpg",
                name: "mxrvpz5f88usxsknazc1",
                order: 2,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[4].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191745/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/v1kl7vgeo2mcvuoqvfjt.jpg",
                publicId: "products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/v1kl7vgeo2mcvuoqvfjt",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191745/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/v1kl7vgeo2mcvuoqvfjt.jpg",
                name: "v1kl7vgeo2mcvuoqvfjt",
                order: 3,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[4].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191747/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/ukhrco0cnag2biy7o09j.jpg",
                publicId: "products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/ukhrco0cnag2biy7o09j",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191747/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/ukhrco0cnag2biy7o09j.jpg",
                name: "ukhrco0cnag2biy7o09j",
                order: 4,
                createdBy: adminId
            ),
            new ProductMedia(
                productId: products[4].Id,
                imageUrl:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191751/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/delel5iyhvk9herqwex2.jpg",
                publicId: "products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/delel5iyhvk9herqwex2",
                path:
                "http://res.cloudinary.com/dus70fkd3/image/upload/v1746191751/products/5be89ed5-c405-4908-a0e9-c5194bb0a384/images/delel5iyhvk9herqwex2.jpg",
                name: "delel5iyhvk9herqwex2",
                order: 5,
                createdBy: adminId
            )
        };

        context.Products.AddRange(products);
        context.ProductMedias.AddRange(productMedia);

        context.SaveChanges();

        #endregion Product List
    }
}