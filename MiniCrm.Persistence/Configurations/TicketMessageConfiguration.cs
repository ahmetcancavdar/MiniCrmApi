using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class TicketMessageConfiguration
    : IEntityTypeConfiguration<TicketMessage>
{
    public void Configure(EntityTypeBuilder<TicketMessage> builder)
    {
        builder.ToTable("TicketMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .HasMaxLength(4000)
            .IsRequired();

        builder.HasIndex(x => x.TicketId);

        builder.HasIndex(x => x.SenderUserId);

        builder.HasIndex(x => new
        {
            x.TicketId,
            x.CreatedAtUtc
        });
    }
}