using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class OrderVerificationConfiguration
    : IEntityTypeConfiguration<OrderVerification>
{
    public void Configure(
        EntityTypeBuilder<OrderVerification> builder)
    {
        builder.ToTable("OrderVerifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodeHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.HasOne(x => x.Order)
            .WithOne(x => x.Verification)
            .HasForeignKey<OrderVerification>(
                x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ExpiresAtUtc);
    }
}