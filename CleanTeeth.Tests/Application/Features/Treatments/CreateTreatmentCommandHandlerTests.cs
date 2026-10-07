using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Treatments.Command.CreateTreatment;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Treatments;

[TestClass]
public class CreateTreatmentCommandHandlerTests
{
    private ITreatmentRepository _treatmentRepository = null!;
    private IAppointmentRepository _appointmentRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CreateTreatmentCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _treatmentRepository = Substitute.For<ITreatmentRepository>();
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateTreatmentCommandHandler(_treatmentRepository, _appointmentRepository, _unitOfWork);
    }

    private static Appointment CreateScheduledAppointment() => new(
        Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
        new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(1)));

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsTreatmentId()
    {
        // Arrange
        var appointment = CreateScheduledAppointment();
        var command = new CreateTreatmentCommand { AppointmentId = appointment.Id };
        _appointmentRepository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);
        _treatmentRepository.ExistsByAppointmentId(appointment.Id, Arg.Any<CancellationToken>()).Returns(false);
        _treatmentRepository.Add(Arg.Any<Treatment>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Treatment>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await _treatmentRepository.Received(1).Add(Arg.Any<Treatment>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_AppointmentNotFound_ThrowsBusinessRuleException()
    {
        // Arrange
        var command = new CreateTreatmentCommand { AppointmentId = Guid.NewGuid() };
        _appointmentRepository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Appointment?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Appointment does not exist.");
    }

    [TestMethod]
    public async Task Handle_TreatmentAlreadyExists_ThrowsBusinessRuleException()
    {
        // Arrange
        var appointment = CreateScheduledAppointment();
        var command = new CreateTreatmentCommand { AppointmentId = appointment.Id };
        _appointmentRepository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);
        _treatmentRepository.ExistsByAppointmentId(appointment.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("This appointment already has a treatment record.");
    }

    [TestMethod]
    public async Task Handle_AppointmentNotScheduled_ThrowsBusinessRuleException()
    {
        // Arrange
        var appointment = CreateScheduledAppointment();
        appointment.Cancel();
        var command = new CreateTreatmentCommand { AppointmentId = appointment.Id };
        _appointmentRepository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);
        _treatmentRepository.ExistsByAppointmentId(appointment.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("This appointment cannot be treated.");
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var appointment = CreateScheduledAppointment();
        var command = new CreateTreatmentCommand { AppointmentId = appointment.Id };
        _appointmentRepository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);
        _treatmentRepository.ExistsByAppointmentId(appointment.Id, Arg.Any<CancellationToken>()).Returns(false);
        _treatmentRepository.Add(Arg.Any<Treatment>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
