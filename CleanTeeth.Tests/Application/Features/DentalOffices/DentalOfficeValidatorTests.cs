using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeeth.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using FluentAssertions;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficeValidatorTests
{
    private static CreateDentalOfficeCommand BuildCreate(
        string name = "Main Office",
        string address = "Office Address",
        string phone = "+4912345670",
        string email = "office@test.com") => new()
        {
            Name = name,
            Address = address,
            Phone = phone,
            Email = email
        };

    [TestMethod]
    public void Create_ValidCommand_IsValid()
    {
        new CreateDentalOfficeCommandValidator().Validate(BuildCreate()).IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Create_EmptyName_IsInvalid()
    {
        var result = new CreateDentalOfficeCommandValidator().Validate(BuildCreate(name: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentalOfficeCommand.Name));
    }

    [TestMethod]
    public void Create_NameTooLong_IsInvalid()
    {
        var result = new CreateDentalOfficeCommandValidator().Validate(BuildCreate(name: new string('a', 151)));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentalOfficeCommand.Name));
    }

    [TestMethod]
    public void Create_InvalidEmail_IsInvalid()
    {
        var result = new CreateDentalOfficeCommandValidator().Validate(BuildCreate(email: "bad-email"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentalOfficeCommand.Email));
    }

    [TestMethod]
    public void Create_PhoneWithInvalidChars_IsInvalid()
    {
        var result = new CreateDentalOfficeCommandValidator().Validate(BuildCreate(phone: "abcdefgh"));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentalOfficeCommand.Phone));
    }

    [TestMethod]
    public void Create_EmptyAddress_IsInvalid()
    {
        var result = new CreateDentalOfficeCommandValidator().Validate(BuildCreate(address: " "));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDentalOfficeCommand.Address));
    }

    [TestMethod]
    public void Update_ValidCommand_IsValid()
    {
        var result = new UpdateDentalOfficeCommandValidator().Validate(new UpdateDentalOfficeCommand
        {
            Id = Guid.NewGuid(),
            Name = "Office",
            Address = "Addr",
            Phone = "+4912345670",
            Email = "office@test.com"
        });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Update_EmptyName_IsInvalid()
    {
        var result = new UpdateDentalOfficeCommandValidator().Validate(new UpdateDentalOfficeCommand
        {
            Id = Guid.NewGuid(),
            Name = " ",
            Address = "Addr",
            Phone = "+4912345670",
            Email = "office@test.com"
        });
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateDentalOfficeCommand.Name));
    }

    [TestMethod]
    public void Delete_ValidId_IsValid()
    {
        var result = new DeleteDentalOfficeCommandValidator().Validate(
            new DeleteDentalOfficeCommand { Id = Guid.NewGuid() });
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Delete_EmptyId_IsInvalid()
    {
        var result = new DeleteDentalOfficeCommandValidator().Validate(
            new DeleteDentalOfficeCommand { Id = Guid.Empty });
        result.IsValid.Should().BeFalse();
    }
}
