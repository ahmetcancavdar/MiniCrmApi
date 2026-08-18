using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class SupportConversationConfiguration
    : IEntityTypeConfiguration<SupportConversation>
{
    public void Configure(
        EntityTypeBuilder<SupportConversation> builder)
    {
        builder.ToTable("SupportConversations");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.SupportConversations)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.SupportConversation)
            .HasForeignKey(x => x.SupportConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Messages)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.CustomerId);

        builder.HasIndex(x => x.OrderId);

        builder.HasIndex(x => new
        {
            x.CustomerId,
            x.Status
        });

        builder.HasIndex(x => x.StartedAtUtc);
    }
}