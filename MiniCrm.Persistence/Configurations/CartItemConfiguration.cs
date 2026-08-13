using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class CartItemConfiguration
    : IEntityTypeConfiguration<CartItem>
{
    public void Configure(
        EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ProductId);

        builder.HasIndex(x =>
                new
                {
                    x.CartId,
                    x.ProductId
                })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}