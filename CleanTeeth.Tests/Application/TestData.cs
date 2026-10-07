using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.ValueObjects;
using System.Reflection;

namespace CleanTeeth.Tests.Application;

/// <summary>
/// 提供构造合法领域对象的工厂方法，供各 Handler/Validator 测试复用，
/// 保证测试数据始终满足实体的业务不变量。
/// </summary>
internal static class TestData
{
    public static Patient NewPatient(
        Guid? userId = null,
        string name = "John Doe",
        string patientNumber = "P-0001") => new(
            userId ?? Guid.NewGuid(),
            name,
            patientNumber,
            new DateOnly(1990, 1, 1),
            Gender.Male,
            new PhoneNumber("+4912345678"),
            new Email("patient@test.com"),
            "Patient Street 1");

    public static Dentist NewDentist(
        Guid? userId = null,
        string name = "Dr. Smith") => new(
            userId ?? Guid.NewGuid(),
            name,
            Gender.Male,
            new PhoneNumber("+4912345679"),
            new Email("dentist@test.com"),
            "LIC-001",
            "Orthodontics");

    public static DentalOffice NewDentalOffice(string name = "Main Office") => new(
        name,
        "Office Street 1",
        new PhoneNumber("+4912345670"),
        new Email("office@test.com"));

    public static Appointment NewAppointment(
        Guid? patientId = null,
        Guid? dentistId = null,
        Guid? dentalOfficeId = null,
        DateTime? start = null)
    {
        var startTime = start ?? DateTime.UtcNow.AddDays(1);
        return new Appointment(
            patientId ?? Guid.NewGuid(),
            dentistId ?? Guid.NewGuid(),
            dentalOfficeId ?? Guid.NewGuid(),
            new TimeInterval(startTime, startTime.AddHours(1)));
    }

    public static Appointment NewAppointmentWithNavigation(
        Patient? patient = null,
        Dentist? dentist = null,
        DentalOffice? dentalOffice = null)
    {
        patient ??= NewPatient();
        dentist ??= NewDentist();
        dentalOffice ??= NewDentalOffice();

        var start = DateTime.UtcNow.AddDays(1);
        var appointment = new Appointment(
            patient.Id,
            dentist.Id,
            dentalOffice.Id,
            new TimeInterval(start, start.AddHours(1)));

        SetPrivate(appointment, nameof(Appointment.Patient), patient);
        SetPrivate(appointment, nameof(Appointment.Dentist), dentist);
        SetPrivate(appointment, nameof(Appointment.DentalOffice), dentalOffice);

        return appointment;
    }

    public static Treatment NewTreatment(Guid? appointmentId = null) =>
        new(appointmentId ?? Guid.NewGuid());

    public static void SetPrivate(object target, string propertyName, object? value)
    {
        var property = target.GetType().GetProperty(
            propertyName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        property?.SetValue(target, value);
    }
}
