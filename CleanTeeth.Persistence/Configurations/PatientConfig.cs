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
        builder.HasIndex(x => x.PatientNumber)
            .IsUnique();
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        builder.Property(p => p.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.ComplexProperty(prop => prop.Email, email =>
        {
            email.Property(e => e.Value)
            .HasColumnName("Email")
            .HasMaxLength(50)
            .IsRequired();
        });
        builder.ComplexProperty(prop => prop.Phone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("Phone")
                .HasMaxLength(30)
                .IsRequired();
        });
        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(500);
    }
}
