using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public sealed class AfterSalesRequestConfiguration
    : IEntityTypeConfiguration<AfterSalesRequest>
{
    public void Configure(
        EntityTypeBuilder<AfterSalesRequest> builder)
    {
        builder.ToTable(
            "AfterSalesRequests");

        builder.HasKey(
            x => x.Id);


        // ============================================================
        // REASON
        // ============================================================

        builder.Property(
                x => x.Reason)
            .HasMaxLength(2000)
            .IsRequired();


        // ============================================================
        // ADMIN NOTE
        // ============================================================

        builder.Property(
                x => x.AdminNote)
            .HasMaxLength(2000);


        // ============================================================
        // INDEXES
        // ============================================================

        builder.HasIndex(
            x => x.CustomerId);

        builder.HasIndex(
            x => new
            {
                x.CustomerId,
                x.Status
            });

        builder.HasIndex(
            x => x.OrderItemId);


        // ============================================================
        // CUSTOMER
        // ============================================================

        builder.HasOne(
                x => x.Customer)
            .WithMany(
                x => x.AfterSalesRequests)
            .HasForeignKey(
                x => x.CustomerId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // ============================================================
        // ORDER ITEM
        // ============================================================

        builder.HasOne(
                x => x.OrderItem)
            .WithMany()
            .HasForeignKey(
                x => x.OrderItemId)
            .OnDelete(
                DeleteBehavior.Restrict);
    }
}