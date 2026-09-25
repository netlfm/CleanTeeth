using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Patient)
            .WithMany()
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Dentist)
            .WithMany()
            .HasForeignKey(x => x.DentistId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DentalOffice)
            .WithMany()
            .HasForeignKey(x => x.DentalOfficeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.ComplexProperty(
            x => x.TimeInterval,
            timeInterval =>
            {
                timeInterval.Property(x => x.Start)
                    .HasColumnName("StartTime");

                timeInterval.Property(x => x.End)
                    .HasColumnName("EndTime");
            });
    }
}
