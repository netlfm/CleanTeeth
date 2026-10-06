using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

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
        office.IsDeleted.Should().BeFalse();
        office.Id.Should().NotBeEmpty();
    }
}
