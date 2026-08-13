using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class OrderConfiguration
    : IEntityTypeConfiguration<Order>
{
    public void Configure(
        EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.CancellationReason)
            .HasMaxLength(500);

        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.HasIndex(x => x.CustomerId);

        builder.HasIndex(x =>
            new
            {
                x.CustomerId,
                x.Status
            });

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);

        builder.ComplexProperty(
            x => x.ShippingAddress,
            address =>
            {
                address.Property(
                        x => x.RecipientName)
                    .HasColumnName(
                        "ShippingRecipientName")
                    .HasMaxLength(150)
                    .IsRequired();

                address.Property(
                        x => x.Phone)
                    .HasColumnName(
                        "ShippingPhone")
                    .HasMaxLength(30);

                address.Property(
                        x => x.AddressLine)
                    .HasColumnName(
                        "ShippingAddressLine")
                    .HasMaxLength(500)
                    .IsRequired();

                address.Property(
                        x => x.City)
                    .HasColumnName(
                        "ShippingCity")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(
                        x => x.District)
                    .HasColumnName(
                        "ShippingDistrict")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(
                        x => x.PostalCode)
                    .HasColumnName(
                        "ShippingPostalCode")
                    .HasMaxLength(20);

                address.Property(
                        x => x.Country)
                    .HasColumnName(
                        "ShippingCountry")
                    .HasMaxLength(100)
                    .IsRequired();
            });
    }
}