using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class SupportMessageConfiguration
    : IEntityTypeConfiguration<SupportMessage>
{
    public void Configure(
        EntityTypeBuilder<SupportMessage> builder)
    {
        builder.ToTable("SupportMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .HasMaxLength(4000)
            .IsRequired();

        builder.HasIndex(x => x.SupportConversationId);

        builder.HasIndex(x => x.SenderUserId);

        builder.HasIndex(x => new
        {
            x.SupportConversationId,
            x.CreatedAtUtc
        });
    }
}