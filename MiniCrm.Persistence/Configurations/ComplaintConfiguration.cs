using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public sealed class ComplaintConfiguration
    : IEntityTypeConfiguration<Complaint>
{
    public void Configure(
        EntityTypeBuilder<Complaint> builder)
    {
        builder.ToTable("Complaints");

        builder.HasKey(x => x.Id);


        // ============================================================
        // CONTENT
        // ============================================================

        builder.Property(x => x.Subject)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.AdminNote)
            .HasMaxLength(2000);


        // ============================================================
        // INDEXES
        // ============================================================

        builder.HasIndex(x =>
            x.CustomerId);

        builder.HasIndex(x =>
            x.OrderId);

        builder.HasIndex(x =>
            x.Status);

        builder.HasIndex(x =>
            new
            {
                x.CustomerId,
                x.Status
            });


        // ============================================================
        // CUSTOMER
        // ============================================================

        builder.HasOne(x =>
                x.Customer)
            .WithMany(x =>
                x.Complaints)
            .HasForeignKey(x =>
                x.CustomerId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // ============================================================
        // ORDER
        // ============================================================

        builder.HasOne(x =>
                x.Order)
            .WithMany()
            .HasForeignKey(x =>
                x.OrderId)
            .OnDelete(
                DeleteBehavior.Restrict);
    }
}