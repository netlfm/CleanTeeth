using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

internal class DentistConfig : IEntityTypeConfiguration<Dentist>
{
    public void Configure(EntityTypeBuilder<Dentist> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.Property(prop => prop.Name)
             .IsRequired()
             .HasMaxLength(150);
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
        builder.Property(prop => prop.Gender)
            .HasMaxLength(20)
            .HasConversion<string>();
        builder.Property(prop => prop.Status)
            .HasMaxLength(30)
            .HasConversion<string>();
        builder.Property(prop => prop.LicenseNumber)
            .HasMaxLength(50);
        builder.Property(prop => prop.Specialty)
            .HasMaxLength(100);
    }
}
