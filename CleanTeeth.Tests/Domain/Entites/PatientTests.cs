using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class PatientTests
{
    private static Patient CreateValidPatient() => new(
        Guid.NewGuid(),
        "Valid Name",
        "P-2026-0001",
        new DateOnly(1990, 5, 1),
        Gender.Male,
        new PhoneNumber("+4912345678"),
        new Email("valid@example.com"),
        "Some Address 1");

    [TestMethod]
    public void Constructor_EmptyUserId_Throws()
    {
        Action act = () => new Patient(Guid.Empty, "Valid Name", "P-1", new DateOnly(1990, 5, 1),
            Gender.Male, new PhoneNumber("+4912345678"), new Email("valid@example.com"), "Some Address 1");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_NullName_Throws()
    {
        Action act = () => new Patient(Guid.NewGuid(), null!, "P-1", new DateOnly(1990, 5, 1),
            Gender.Male, new PhoneNumber("+4912345678"), new Email("valid@example.com"), "Some Address 1");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_EmptyPatientNumber_Throws()
    {
        Action act = () => new Patient(Guid.NewGuid(), "Valid Name", " ", new DateOnly(1990, 5, 1),
            Gender.Male, new PhoneNumber("+4912345678"), new Email("valid@example.com"), "Some Address 1");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_ValidParameters_SetsProperties()
    {
        var userId = Guid.NewGuid();
        var patient = CreateValidPatient() with { };
        var created = new Patient(userId, "Jane Doe", "P-2026-0002", new DateOnly(1985, 3, 10),
            Gender.Female, new PhoneNumber("+4912345679"), new Email("jane@example.com"), "Other Address 2");

        created.UserId.Should().Be(userId);
        created.Name.Should().Be("Jane Doe");
        created.PatientNumber.Should().Be("P-2026-0002");
        created.Status.Should().NotBe("deleted");
        created.IsDeleted.Should().BeFalse();
        created.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Delete_SetsSoftDeleteFields()
    {
        var patient = CreateValidPatient();
        var actorId = Guid.NewGuid();

        patient.Delete(actorId);

        patient.IsDeleted.Should().BeTrue();
        patient.DeletedBy.Should().Be(actorId);
        patient.DeletedAt.Should().NotBeNull();
    }
}
