using CleanTeeth.Application.Features.Patients.Commands.CreatePatient;
using CleanTeeth.Application.Features.Patients.Commands.DeletePatient;
using CleanTeeth.Application.Features.Patients.Commands.UpdatePatient;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using CleanTeeth.Domain.Enums;
using FluentAssertions;

namespace CleanTeeth.Tests.Application.Features.Patients;

[TestClass]
public class PatientValidatorTests
{
    private static CreatePatientCommand BuildCreate(
        Guid? userId = null,
        string name = "John Doe",
        DateOnly? dateOfBirth = null,
        Gender? gender = null,
        string phone = "+4912345678",
        string email = "john@test.com",
        string address = "Some Address") => new()
        {
            UserId = userId ?? Guid.NewGuid(),
            Name = name,
            DateOfBirth = dateOfBirth ?? new DateOnly(1990, 1, 1),
            Gender = gender ?? Gender.Male,
            Phone = phone,
            Email = email,
            Address = address
        };

    // ---------- CreatePatient ----------

    [TestMethod]
    public void Create_ValidCommand_IsValid()
    {
        new CreatePatientCommandValidator().Validate(BuildCreate()).IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Create_EmptyUserId_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(userId: Guid.Empty));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.UserId));
    }

    [TestMethod]
    public void Create_EmptyName_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(name: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Name));
    }

    [TestMethod]
    public void Create_NameTooLong_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(name: new string('a', 201)));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Name));
    }

    [TestMethod]
    public void Create_InvalidGender_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(gender: (Gender)999));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Gender));
    }

    [TestMethod]
    public void Create_PhoneWithLetters_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(phone: "abcdefgh"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Phone));
    }

    [TestMethod]
    public void Create_InvalidEmail_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(email: "not-an-email"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Email));
    }

    [TestMethod]
    public void Create_EmptyAddress_IsInvalid()
    {
        var result = new CreatePatientCommandValidator().Validate(BuildCreate(address: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Address));
    }

    // ---------- UpdatePatient ----------

    private static UpdatePatientCommand BuildUpdate(
        string name = "John",
        string email = "john@test.com") => new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            DateOfBirth = new DateOnly(1990, 1, 1),
            Gender = Gender.Male,
            Phone = "+4912345678",
            Email = email,
            Address = "Addr"
        };

    [TestMethod]
    public void Update_ValidCommand_IsValid()
    {
        new UpdatePatientCommandValidator().Validate(BuildUpdate()).IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Update_EmptyName_IsInvalid()
    {
        var result = new UpdatePatientCommandValidator().Validate(BuildUpdate(name: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdatePatientCommand.Name));
    }

    [TestMethod]
    public void Update_InvalidEmail_IsInvalid()
    {
        var result = new UpdatePatientCommandValidator().Validate(BuildUpdate(email: "bad"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdatePatientCommand.Email));
    }

    // ---------- DeletePatient ----------

    [TestMethod]
    public void Delete_ValidId_IsValid()
    {
        var result = new DeletePatientCommandValidator().Validate(
            new DeletePatientCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Delete_EmptyId_IsInvalid()
    {
        var result = new DeletePatientCommandValidator().Validate(
            new DeletePatientCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(DeletePatientCommand.Id));
    }

    // ---------- Get list / detail ----------

    [TestMethod]
    public void GetList_ValidPaging_IsValid()
    {
        var result = new GetPatientListQueryValidator().Validate(
            new GetPatientListQuery { PageNumber = 1, PageSize = 10 });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void GetList_PageNumberZero_IsInvalid()
    {
        var result = new GetPatientListQueryValidator().Validate(
            new GetPatientListQuery { PageNumber = 0, PageSize = 10 });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetPatientListQuery.PageNumber));
    }

    [TestMethod]
    public void GetList_InvalidGenderFilter_IsInvalid()
    {
        var result = new GetPatientListQueryValidator().Validate(
            new GetPatientListQuery { PageNumber = 1, PageSize = 10, Gender = (Gender)999 });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetPatientListQuery.Gender));
    }

    [TestMethod]
    public void GetDetail_EmptyId_IsInvalid()
    {
        var result = new GetPatientDetailQueryValidator().Validate(
            new GetPatientDetailQuery { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }
}
