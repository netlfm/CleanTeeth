using CleanTeeth.Application.Features.Appointments.Commands.CancelAppointment;
using CleanTeeth.Application.Features.Appointments.Commands.CompleteAppointment;
using CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDetail;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using FluentAssertions;

namespace CleanTeeth.Tests.Application.Features.Appointments;

[TestClass]
public class AppointmentValidatorTests
{
    private static CreateAppointmentCommand BuildCreate(
        DateTime? startDate = null,
        DateTime? endDate = null) => new()
        {
            PatientId = Guid.NewGuid(),
            DentistId = Guid.NewGuid(),
            DentalOfficeId = Guid.NewGuid(),
            StartDate = startDate ?? DateTime.UtcNow.AddDays(1),
            EndDate = endDate ?? DateTime.UtcNow.AddDays(1).AddHours(1)
        };

    [TestMethod]
    public void Create_ValidCommand_IsValid()
    {
        new CreateAppointmentCommandValidator().Validate(BuildCreate()).IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Create_EndBeforeStart_IsInvalid()
    {
        var result = new CreateAppointmentCommandValidator().Validate(BuildCreate(
            startDate: DateTime.UtcNow.AddDays(2),
            endDate: DateTime.UtcNow.AddDays(1)));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateAppointmentCommand.StartDate));
    }

    [TestMethod]
    public void Cancel_ValidId_IsValid()
    {
        var result = new CancelAppointmentCommandValidator().Validate(
            new CancelAppointmentCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Cancel_EmptyId_IsInvalid()
    {
        var result = new CancelAppointmentCommandValidator().Validate(
            new CancelAppointmentCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void Complete_ValidId_IsValid()
    {
        var result = new CompleteAppointmentCommandValidator().Validate(
            new CompleteAppointmentCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Complete_EmptyId_IsInvalid()
    {
        var result = new CompleteAppointmentCommandValidator().Validate(
            new CompleteAppointmentCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void GetList_ValidPaging_IsValid()
    {
        var result = new GetAppointmentListQueryValidator().Validate(
            new GetAppointmentListQuery { PageNumber = 1, PageSize = 10 });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void GetList_PageNumberZero_IsInvalid()
    {
        var result = new GetAppointmentListQueryValidator().Validate(
            new GetAppointmentListQuery { PageNumber = 0, PageSize = 10 });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetAppointmentListQuery.PageNumber));
    }

    [TestMethod]
    public void GetDetail_EmptyId_IsInvalid()
    {
        var result = new GetAppointmentDetailQueryValidator().Validate(
            new GetAppointmentDetailQuery { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }
}
