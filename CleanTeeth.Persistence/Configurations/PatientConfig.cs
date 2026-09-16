using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

public class PatientConfig : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PatientNumber)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        builder.Property(p => p.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.OwnsOne(p => p.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();
        });
        builder.OwnsOne(p => p.Phone, phoneBuilder =>
        {
            phoneBuilder.Property(ph => ph.Value)
                .HasColumnName("Phone")
                .HasMaxLength(30)
                .IsRequired();
        });
        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(500);
    }
}
