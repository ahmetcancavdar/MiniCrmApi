using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniCrm.Domain.Common;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Identity;

namespace MiniCrm.Persistence.Context;

public class AppDbContext
    : IdentityDbContext<
        ApplicationUser,
        IdentityRole<Guid>,
        Guid>
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers =>
        Set<Customer>();

    public DbSet<CustomerAddress> CustomerAddresses =>
        Set<CustomerAddress>();

    public DbSet<Category> Categories =>
        Set<Category>();

    public DbSet<Product> Products =>
        Set<Product>();

    public DbSet<StockMovement> StockMovements =>
        Set<StockMovement>();

    public DbSet<Cart> Carts =>
        Set<Cart>();

    public DbSet<CartItem> CartItems =>
        Set<CartItem>();

    public DbSet<Order> Orders =>
        Set<Order>();

    public DbSet<OrderItem> OrderItems =>
        Set<OrderItem>();

    public DbSet<OrderVerification> OrderVerifications =>
        Set<OrderVerification>();

    public DbSet<SupportConversation> SupportConversations =>
        Set<SupportConversation>();

    public DbSet<SupportMessage> SupportMessages =>
        Set<SupportMessage>();

    public DbSet<EmailLog> EmailLogs =>
        Set<EmailLog>();

    public DbSet<Lead> Leads =>
        Set<Lead>();

    public DbSet<LeadNote> LeadNotes =>
        Set<LeadNote>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        ApplySoftDeleteQueryFilters(builder);
    }

    public override int SaveChanges()
    {
        ApplyAuditInformation();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();

        return base.SaveChangesAsync(
            cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        var utcNow =
            DateTime.UtcNow;

        foreach (var entry in
                 ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    entry.Entity.CreatedAtUtc =
                        utcNow;

                    entry.Entity.UpdatedAtUtc =
                        null;

                    entry.Entity.IsDeleted =
                        false;

                    entry.Entity.DeletedAtUtc =
                        null;

                    break;


                case EntityState.Modified:

                    entry.Entity.UpdatedAtUtc =
                        utcNow;

                    if (entry.Entity.IsDeleted &&
                        entry.Entity.DeletedAtUtc is null)
                    {
                        entry.Entity.DeletedAtUtc =
                            utcNow;
                    }

                    break;


                case EntityState.Deleted:

                    entry.State =
                        EntityState.Modified;

                    entry.Entity.IsDeleted =
                        true;

                    entry.Entity.DeletedAtUtc =
                        utcNow;

                    entry.Entity.UpdatedAtUtc =
                        utcNow;

                    break;
            }
        }
    }

    private static void ApplySoftDeleteQueryFilters(
        ModelBuilder builder)
    {
        foreach (var entityType in
                 builder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity)
                .IsAssignableFrom(
                    entityType.ClrType))
            {
                continue;
            }

            var parameter =
                Expression.Parameter(
                    entityType.ClrType,
                    "entity");

            var isDeletedProperty =
                Expression.Property(
                    parameter,
                    nameof(BaseEntity.IsDeleted));

            var comparison =
                Expression.Equal(
                    isDeletedProperty,
                    Expression.Constant(false));

            var lambda =
                Expression.Lambda(
                    comparison,
                    parameter);

            entityType.SetQueryFilter(
                lambda);
        }
    }
}