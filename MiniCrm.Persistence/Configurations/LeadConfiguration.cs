using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Persistence.Configurations;

public class LeadConfiguration
    : IEntityTypeConfiguration<Lead>
{
    public void Configure(
        EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(30);

        builder.Property(x => x.Source)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.InterestArea)
            .HasMaxLength(250);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasIndex(x => x.Email);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.Source);

        builder.HasIndex(x => x.AssignedAdminUserId);

        builder.HasIndex(x => x.NextFollowUpDate);

        builder.HasOne(x => x.ConvertedCustomer)
            .WithMany()
            .HasForeignKey(x => x.ConvertedCustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.LeadNotes)
            .WithOne(x => x.Lead)
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.LeadNotes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
