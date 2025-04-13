using HeadphoneStore.Domain.Entities.Content;
using HeadphoneStore.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers => Set<AppUser>();
    public virtual DbSet<AppRole> AppRoles => Set<AppRole>();
    public virtual DbSet<Product> Products => Set<Product>();
    public virtual DbSet<ProductMedia> ProductMedias => Set<ProductMedia>();
    public virtual DbSet<Category> Categories => Set<Category>();
    public virtual DbSet<Order> Orders => Set<Order>();
    public virtual DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public virtual DbSet<OrderPayment> OrderPayments => Set<OrderPayment>();
    public virtual DbSet<Transaction> Transactions => Set<Transaction>();
    public virtual DbSet<UserAddress> UserAddresses => Set<UserAddress>();

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
}
