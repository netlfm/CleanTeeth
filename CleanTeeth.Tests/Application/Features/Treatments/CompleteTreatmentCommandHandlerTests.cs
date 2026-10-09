using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;
using CleanTeeth.Application.Notifications;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using System.Reflection;

namespace CleanTeeth.Tests.Application.Features.Treatments;

[TestClass]
public class CompleteTreatmentCommandHandlerTests
{
    private ITreatmentRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private INotifications _notifications = null!;
    private CompleteTreatmentCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<ITreatmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _notifications = Substitute.For<INotifications>();
        _handler = new CompleteTreatmentCommandHandler(_repository, _unitOfWork, _notifications);
    }

    private static Treatment CreateTreatmentWithNavigation()
    {
        var patient = new Patient(
            Guid.NewGuid(), "Test Patient", "P-001",
            new DateOnly(1990, 1, 1), Gender.Male,
            new PhoneNumber("+4912345678"), new Email("patient@test.com"),
            "Patient Address");
        var dentist = new Dentist(
            Guid.NewGuid(), "Test Dentist", Gender.Male,
            new PhoneNumber("+4912345679"), new Email("dentist@test.com"),
            "LIC-001", "Orthodontics");
        var dentalOffice = new DentalOffice(
            "Test Office", "Office Address",
            new PhoneNumber("+4912345670"), new Email("office@test.com"));
        var appointment = new Appointment(
            patient.Id, dentist.Id, dentalOffice.Id,
            new TimeInterval(DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2)));
        SetPrivateProperty(appointment, nameof(Appointment.Patient), patient);
        SetPrivateProperty(appointment, nameof(Appointment.Dentist), dentist);
        SetPrivateProperty(appointment, nameof(Appointment.DentalOffice), dentalOffice);

        var treatment = new Treatment(appointment.Id);
        SetPrivateProperty(treatment, nameof(Treatment.Appointment), appointment);
        return treatment;
    }

    private static void SetPrivateProperty(object target, string propertyName, object? value)
    {
        var property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        property?.SetValue(target, value);
    }

    [TestMethod]
    public async Task Handle_ExistingTreatment_CompletesAndCommits()
    {
        // Arrange
        var treatment = CreateTreatmentWithNavigation();
        var command = new CompleteTreatmentCommand { Id = treatment.Id, Notes = "Done successfully" };
        _repository.GetById(treatment.Id, Arg.Any<CancellationToken>()).Returns(treatment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        treatment.Status.Should().Be(TreatmentStatus.Completed);
        treatment.Notes.Should().Be("Done successfully");
        await _repository.Received(1).Update(treatment, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_ExistingTreatment_SendsTreatmentReportNotification()
    {
        // Arrange
        var treatment = CreateTreatmentWithNavigation();
        var command = new CompleteTreatmentCommand { Id = treatment.Id, Notes = "Done successfully" };
        _repository.GetById(treatment.Id, Arg.Any<CancellationToken>()).Returns(treatment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _notifications.Received(1).SendTreatmentReport(Arg.Is<TreatmentReportDTO>(dto =>
            dto.Id == treatment.Id &&
            dto.DurationMinutes == treatment.DurationMinutes &&
            dto.Notes == "Done successfully"));
    }

    [TestMethod]
    public async Task Handle_TreatmentNotFound_ThrowsBusinessRuleException()
    {
        // Arrange
        var command = new CompleteTreatmentCommand { Id = Guid.NewGuid(), Notes = "test" };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Treatment?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Treatment does not exist.");
        await _repository.DidNotReceive().Update(Arg.Any<Treatment>(), Arg.Any<CancellationToken>());
        await _notifications.DidNotReceive().SendTreatmentReport(Arg.Any<TreatmentReportDTO>());
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var treatment = CreateTreatmentWithNavigation();
        var command = new CompleteTreatmentCommand { Id = treatment.Id, Notes = "test" };
        _repository.GetById(treatment.Id, Arg.Any<CancellationToken>()).Returns(treatment);
        _repository.Update(Arg.Any<Treatment>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
        await _notifications.DidNotReceive().SendTreatmentReport(Arg.Any<TreatmentReportDTO>());
    }
}
