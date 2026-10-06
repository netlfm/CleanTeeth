using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class DentistTests
{
    private static Dentist CreateValidDentist() => new(
        Guid.NewGuid(),
        "Dr. Valid",
        Gender.Male,
        new PhoneNumber("+4912345678"),
        new Email("doc@example.com"),
        "LIC-123",
        "Orthodontics");

    [TestMethod]
    public void Constructor_EmptyUserId_Throws()
    {
        Action act = () => new Dentist(Guid.Empty, "Dr. Valid", Gender.Male,
            new PhoneNumber("+4912345678"), new Email("doc@example.com"), null, null);
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_NullName_Throws()
    {
        Action act = () => new Dentist(Guid.NewGuid(), null!, Gender.Male,
            new PhoneNumber("+4912345678"), new Email("doc@example.com"), null, null);
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_NullEmail_Throws()
    {
        Action act = () => new Dentist(Guid.NewGuid(), "Dr. Valid", Gender.Male,
            new PhoneNumber("+4912345678"), null!, null, null);
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_ValidParameters_IsActive()
    {
        var dentist = CreateValidDentist();

        dentist.Status.Should().Be(DentistStatus.Active);
        dentist.UserId.Should().NotBeEmpty();
        dentist.LicenseNumber.Should().Be("LIC-123");
        dentist.Specialty.Should().Be("Orthodontics");
    }

    [TestMethod]
    public void ChangeStatus_SameStatus_NoChange()
    {
        var dentist = CreateValidDentist();

        dentist.ChangeStatus(DentistStatus.Active);

        dentist.Status.Should().Be(DentistStatus.Active);
    }
}
