using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class LeadNoteConfiguration
    : IEntityTypeConfiguration<LeadNote>
{
    public void Configure(
        EntityTypeBuilder<LeadNote> builder)
    {
        builder.ToTable("LeadNotes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AdminUserId)
            .IsRequired();

        builder.Property(x => x.Note)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasIndex(x => x.LeadId);
    }
}
