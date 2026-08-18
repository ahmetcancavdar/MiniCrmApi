using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class EmailLogConfiguration
    : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.ToTable("EmailLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ToEmail)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Subject)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Body)
            .IsRequired();

        builder.Property(x => x.FailureReason)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.EmailLogs)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CustomerId);

        builder.HasIndex(x => x.OrderId);

        builder.HasIndex(x => new
        {
            x.DeliveryStatus,
            x.CreatedAtUtc
        });
    }
}