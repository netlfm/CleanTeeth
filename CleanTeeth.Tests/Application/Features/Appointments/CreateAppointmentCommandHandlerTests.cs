using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;
using CleanTeeth.Application.Notifications;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using System.Reflection;

namespace CleanTeeth.Tests.Application.Features.Appointments;

[TestClass]
public class CreateAppointmentCommandHandlerTests
{
    private IAppointmentRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private INotifications _notifications = null!;
    private CreateAppointmentCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IAppointmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _notifications = Substitute.For<INotifications>();
        _handler = new CreateAppointmentCommandHandler(_repository, _unitOfWork, _notifications);
    }

    private static CreateAppointmentCommand CreateValidCommand() => new()
    {
        PatientId = Guid.NewGuid(),
        DentistId = Guid.NewGuid(),
        DentalOfficeId = Guid.NewGuid(),
        StartDate = DateTime.UtcNow.AddDays(1),
        EndDate = DateTime.UtcNow.AddDays(1).AddHours(1)
    };

    private static Appointment CreateAppointmentWithNavigation(CreateAppointmentCommand command)
    {
        var appointment = new Appointment(
            command.PatientId, command.DentistId, command.DentalOfficeId,
            new TimeInterval(command.StartDate, command.EndDate));

        var patient = new Patient(
            command.PatientId, "Test Patient", "P-001",
            new DateOnly(1990, 1, 1), Gender.Male,
            new PhoneNumber("+4912345678"), new Email("patient@test.com"),
            "Patient Address");
        var dentist = new Dentist(
            command.DentistId, "Test Dentist", Gender.Male,
            new PhoneNumber("+4912345679"), new Email("dentist@test.com"),
            "LIC-001", "Orthodontics");
        var dentalOffice = new DentalOffice(
            "Test Office", "Office Address",
            new PhoneNumber("+4912345670"), new Email("office@test.com"));

        SetPrivateProperty(appointment, nameof(Appointment.Patient), patient);
        SetPrivateProperty(appointment, nameof(Appointment.Dentist), dentist);
        SetPrivateProperty(appointment, nameof(Appointment.DentalOffice), dentalOffice);

        return appointment;
    }

    private static void SetPrivateProperty(object target, string propertyName, object? value)
    {
        var property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        property?.SetValue(target, value);
    }

    [TestMethod]
    public async Task Handle_NoConflict_ReturnsAppointmentId()
    {
        // Arrange
        var command = CreateValidCommand();
        var appointment = CreateAppointmentWithNavigation(command);
        _repository.FindConflict(
            Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(),
            Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns((Appointment?)null);
        _repository.Add(Arg.Any<Appointment>(), Arg.Any<CancellationToken>()).Returns(appointment);
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(appointment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(appointment.Id);
        await _repository.Received(1).Add(Arg.Any<Appointment>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_ConflictExists_ThrowsAppointmentOverlapException()
    {
        // Arrange
        var command = CreateValidCommand();
        var conflict = new Appointment(
            command.PatientId, command.DentistId, command.DentalOfficeId,
            new TimeInterval(command.StartDate, command.EndDate));
        _repository.FindConflict(
            Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(),
            Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(conflict);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AppointmentOverlapException>();
        await _repository.DidNotReceive().Add(Arg.Any<Appointment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var command = CreateValidCommand();
        _repository.FindConflict(
            Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(),
            Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns((Appointment?)null);
        _repository.Add(Arg.Any<Appointment>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
