using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Appointments.Commands.CompleteAppointment;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Appointments;

[TestClass]
public class CompleteAppointmentCommandHandlerTests
{
    private IAppointmentRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CompleteAppointmentCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IAppointmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CompleteAppointmentCommandHandler(_repository, _unitOfWork);
    }

    private static Appointment CreateScheduledAppointment() => new(
        Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
        new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(1)));

    [TestMethod]
    public async Task Handle_ExistingAppointment_CompletesAndCommits()
    {
        // Arrange
        var appointment = CreateScheduledAppointment();
        var command = new CompleteAppointmentCommand { Id = appointment.Id };
        _repository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        appointment.Status.Should().Be(CleanTeeth.Domain.Enums.AppointmentStatus.Completed);
        await _repository.Received(1).Update(appointment, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_AppointmentNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new CompleteAppointmentCommand { Id = Guid.NewGuid() };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Appointment?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().Update(Arg.Any<Appointment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var appointment = CreateScheduledAppointment();
        var command = new CompleteAppointmentCommand { Id = appointment.Id };
        _repository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);
        _repository.Update(Arg.Any<Appointment>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
