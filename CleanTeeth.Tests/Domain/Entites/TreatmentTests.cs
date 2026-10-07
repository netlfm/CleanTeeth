using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class TreatmentTests
{
    private readonly Guid _appointmentId = Guid.NewGuid();

    [TestMethod]
    public void Constructor_ValidAppointmentId_SetsInProgressStatus()
    {
        var treatment = new Treatment(_appointmentId);

        treatment.AppointmentId.Should().Be(_appointmentId);
        treatment.Status.Should().Be(TreatmentStatus.InProgress);
        treatment.StartTime.Should().NotBeNull();
        treatment.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public void CompleteTreatment_InProgress_ChangesStatusToCompleted()
    {
        var treatment = new Treatment(_appointmentId);
        var endTime = treatment.StartTime!.Value.AddMinutes(30);

        treatment.CompleteTreatment(endTime, "All good");

        treatment.Status.Should().Be(TreatmentStatus.Completed);
        treatment.DurationMinutes.Should().Be(30);
        treatment.Notes.Should().Be("All good");
    }

    [TestMethod]
    public void CompleteTreatment_NotInProgress_Throws()
    {
        var treatment = new Treatment(_appointmentId);
        treatment.CancelTreatment();
        var endTime = DateTime.UtcNow.AddMinutes(30);

        Action act = () => treatment.CompleteTreatment(endTime, null);

        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void CancelTreatment_InProgress_ChangesStatusToCancelled()
    {
        var treatment = new Treatment(_appointmentId);

        treatment.CancelTreatment();

        treatment.Status.Should().Be(TreatmentStatus.Cancelled);
    }

    [TestMethod]
    public void CancelTreatment_Completed_Throws()
    {
        var treatment = new Treatment(_appointmentId);
        treatment.CompleteTreatment(treatment.StartTime!.Value.AddMinutes(10), "Done");

        Action act = () => treatment.CancelTreatment();

        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Completed treatment cannot be cancelled.");
    }

    [TestMethod]
    public void CancelTreatment_AlreadyCancelled_Throws()
    {
        var treatment = new Treatment(_appointmentId);
        treatment.CancelTreatment();

        Action act = () => treatment.CancelTreatment();

        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Treatment has already been cancelled.");
    }
}
