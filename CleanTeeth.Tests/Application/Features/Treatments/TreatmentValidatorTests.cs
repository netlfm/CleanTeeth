using CleanTeeth.Application.Features.Treatments.Command.CancelTreatment;
using CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;
using CleanTeeth.Application.Features.Treatments.Command.CreateTreatment;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;
using FluentAssertions;

namespace CleanTeeth.Tests.Application.Features.Treatments;

[TestClass]
public class TreatmentValidatorTests
{
    [TestMethod]
    public void Create_ValidAppointmentId_IsValid()
    {
        var result = new CreateTreatmentCommandValidator().Validate(
            new CreateTreatmentCommand { AppointmentId = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Create_EmptyAppointmentId_IsInvalid()
    {
        var result = new CreateTreatmentCommandValidator().Validate(
            new CreateTreatmentCommand { AppointmentId = Guid.Empty });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTreatmentCommand.AppointmentId));
    }

    [TestMethod]
    public void Cancel_ValidId_IsValid()
    {
        var result = new CancelTreatmentCommandValidator().Validate(
            new CancelTreatmentCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Cancel_EmptyId_IsInvalid()
    {
        var result = new CancelTreatmentCommandValidator().Validate(
            new CancelTreatmentCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void Complete_ValidCommand_IsValid()
    {
        var result = new CompleteTreatmentCommandValidator().Validate(
            new CompleteTreatmentCommand { Id = Guid.NewGuid(), Notes = "Completed fine" });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Complete_EmptyNotes_IsInvalid()
    {
        var result = new CompleteTreatmentCommandValidator().Validate(
            new CompleteTreatmentCommand { Id = Guid.NewGuid(), Notes = " " });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CompleteTreatmentCommand.Notes));
    }

    [TestMethod]
    public void Complete_NotesTooLong_IsInvalid()
    {
        var result = new CompleteTreatmentCommandValidator().Validate(
            new CompleteTreatmentCommand { Id = Guid.NewGuid(), Notes = new string('a', 1001) });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CompleteTreatmentCommand.Notes));
    }

    [TestMethod]
    public void GetList_ValidPaging_IsValid()
    {
        var result = new GetTreatmentListQueryValidator().Validate(
            new GetTreatmentListQuery { PageNumber = 1, PageSize = 10 });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void GetList_PageSizeZero_IsInvalid()
    {
        var result = new GetTreatmentListQueryValidator().Validate(
            new GetTreatmentListQuery { PageNumber = 1, PageSize = 0 });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetTreatmentListQuery.PageSize));
    }

    // 注意：该 Validator 的泛型参数是 GetTreatmentDetailResponse（校验 Response 而非 Query）
    [TestMethod]
    public void GetDetailValidator_ResponseWithId_IsValid()
    {
        var result = new GetTreatmentDetailQueryValidator().Validate(
            new GetTreatmentDetailResponse { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void GetDetailValidator_EmptyResponseId_IsInvalid()
    {
        var result = new GetTreatmentDetailQueryValidator().Validate(
            new GetTreatmentDetailResponse { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }
}
