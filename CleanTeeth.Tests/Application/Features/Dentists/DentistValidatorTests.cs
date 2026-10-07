using CleanTeeth.Application.Features.Dentists.Commands.ActiveDentist;
using CleanTeeth.Application.Features.Dentists.Commands.CreateDentist;
using CleanTeeth.Application.Features.Dentists.Commands.DeleteDentist;
using CleanTeeth.Application.Features.Dentists.Commands.InactiveDentist;
using CleanTeeth.Application.Features.Dentists.Commands.OnLeaveDentist;
using CleanTeeth.Application.Features.Dentists.Commands.UpdateDentist;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using CleanTeeth.Domain.Enums;
using FluentAssertions;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class DentistValidatorTests
{
    private static CreateDentistCommand BuildCreate(
        Guid? userId = null,
        string name = "Dr. Smith",
        Gender? gender = null,
        string phone = "+4912345679",
        string email = "smith@test.com",
        string? licenseNumber = "LIC-001",
        string? specialty = "Orthodontics") => new()
        {
            UserId = userId ?? Guid.NewGuid(),
            Name = name,
            Gender = gender ?? Gender.Male,
            Phone = phone,
            Email = email,
            LicenseNumber = licenseNumber,
            Specialty = specialty
        };

    [TestMethod]
    public void Create_ValidCommand_IsValid()
    {
        new CreateDentistCommandValidator().Validate(BuildCreate()).IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Create_EmptyUserId_IsInvalid()
    {
        var result = new CreateDentistCommandValidator().Validate(BuildCreate(userId: Guid.Empty));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentistCommand.UserId));
    }

    [TestMethod]
    public void Create_EmptyName_IsInvalid()
    {
        var result = new CreateDentistCommandValidator().Validate(BuildCreate(name: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentistCommand.Name));
    }

    [TestMethod]
    public void Create_InvalidEmail_IsInvalid()
    {
        var result = new CreateDentistCommandValidator().Validate(BuildCreate(email: "bad"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentistCommand.Email));
    }

    [TestMethod]
    public void Create_LicenseWithInvalidChars_IsInvalid()
    {
        var result = new CreateDentistCommandValidator().Validate(BuildCreate(licenseNumber: "BAD_LIC!"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentistCommand.LicenseNumber));
    }

    [TestMethod]
    public void Create_InvalidGender_IsInvalid()
    {
        var result = new CreateDentistCommandValidator().Validate(BuildCreate(gender: (Gender)999));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentistCommand.Gender));
    }

    private static UpdateDentistCommand BuildUpdate(string name = "Dr. Smith") => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        Gender = Gender.Male,
        Phone = "+4912345679",
        Email = "smith@test.com",
        LicenseNumber = "LIC-001",
        Specialty = "Orthodontics"
    };

    [TestMethod]
    public void Update_ValidCommand_IsValid()
    {
        new UpdateDentistCommandValidator().Validate(BuildUpdate()).IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Update_EmptyName_IsInvalid()
    {
        var result = new UpdateDentistCommandValidator().Validate(BuildUpdate(name: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateDentistCommand.Name));
    }

    [TestMethod]
    public void Delete_EmptyId_IsInvalid()
    {
        var result = new DeleteDentistCommandValidator().Validate(
            new DeleteDentistCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void Active_ValidId_IsValid()
    {
        var result = new ActiveDentistCommandValidator().Validate(
            new ActiveDentistCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Inactive_EmptyId_IsInvalid()
    {
        var result = new InactiveDentistCommandValidator().Validate(
            new InactiveDentistCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void OnLeave_ValidId_IsValid()
    {
        var result = new OnLeaveDentistCommandValidator().Validate(
            new OnLeaveDentistCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    // ---------- Queries ----------

    [TestMethod]
    public void GetList_ValidPaging_IsValid()
    {
        var result = new GetDentistListQueryValidator().Validate(
            new GetDentistListQuery { PageNumber = 1, PageSize = 10 });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void GetList_PageSizeZero_IsInvalid()
    {
        var result = new GetDentistListQueryValidator().Validate(
            new GetDentistListQuery { PageNumber = 1, PageSize = 0 });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetDentistListQuery.PageSize));
    }

    [TestMethod]
    public void GetDetail_EmptyId_IsInvalid()
    {
        var result = new GetDentistDetailQueryValidator().Validate(
            new GetDentistDetailQuery { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }
}
