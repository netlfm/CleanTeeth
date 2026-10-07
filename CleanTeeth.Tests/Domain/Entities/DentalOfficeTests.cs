using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entities;

[TestClass]
public class DentalOfficeTests
{
    [TestMethod]
    public void Constructor_NullName_Throws()
    {
        Action act = () => new DentalOffice(null!, "Address", new PhoneNumber("+4912345678"), new Email("o@t.com"));
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_NullAddress_Throws()
    {
        Action act = () => new DentalOffice("Office", null!, new PhoneNumber("+4912345678"), new Email("o@t.com"));
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_NullPhone_Throws()
    {
        Action act = () => new DentalOffice("Office", "Address", null!, new Email("o@t.com"));
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_ValidParameters_SetsProperties()
    {
        var office = new DentalOffice("Main Office", "Hauptstr. 1", new PhoneNumber("+4912345678"), new Email("office@t.com"));

        office.Name.Should().Be("Main Office");
        office.Address.Should().Be("Hauptstr. 1");
        office.IsDeleted.Should().BeFalse();
        office.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Constructor_NullEmail_Throws()
    {
        Action act = () => new DentalOffice("Office", "Address", new PhoneNumber("+4912345678"), null!);
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Update_ValidParameters_UpdatesProperties()
    {
        var office = new DentalOffice("Old Name", "Old Address", new PhoneNumber("+4912345678"), new Email("old@t.com"));
        var newPhone = new PhoneNumber("+4998765432");
        var newEmail = new Email("new@t.com");

        office.Update("New Name", "New Address", newPhone, newEmail);

        office.Name.Should().Be("New Name");
        office.Address.Should().Be("New Address");
        office.Phone.Should().Be(newPhone);
        office.Email.Should().Be(newEmail);
    }

    [TestMethod]
    public void Update_EmptyName_Throws()
    {
        var office = new DentalOffice("Office", "Address", new PhoneNumber("+4912345678"), new Email("o@t.com"));

        Action act = () => office.Update(" ", "Address", new PhoneNumber("+4912345678"), new Email("o@t.com"));

        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Delete_SetsSoftDeleteFields()
    {
        var office = new DentalOffice("Office", "Address", new PhoneNumber("+4912345678"), new Email("o@t.com"));
        var actorId = Guid.NewGuid();

        office.Delete(actorId);

        office.IsDeleted.Should().BeTrue();
        office.DeletedBy.Should().Be(actorId);
        office.DeletedAt.Should().NotBeNull();
    }
}
