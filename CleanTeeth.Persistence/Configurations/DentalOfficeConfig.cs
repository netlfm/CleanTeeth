using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

public class DentalOfficeConfig : IEntityTypeConfiguration<DentalOffice>
{
    public void Configure(EntityTypeBuilder<DentalOffice> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.Property(prop => prop.Name)
            .HasMaxLength(150)
            .IsRequired();
        builder.ComplexProperty(prop => prop.Email, email =>
        {
            email.Property(e => e.Value)
            .HasColumnName("Email")
            .HasMaxLength(50)
            .IsRequired();
        });
        builder.Property(prop => prop.Address)
            .HasMaxLength(200)
            .IsRequired();
        builder.ComplexProperty(prop => prop.Phone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("Phone")
                .HasMaxLength(30)
                .IsRequired();
        });
    }
}
