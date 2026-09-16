using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class DentistTests
{
    [TestMethod]
    public void Constructor_NullName_Throws()
    {
        var email = new Email("valid@example.com");
        Action act = () => new Dentist(null!, email);
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Constructor_NullEmail_Throws()
    {
        Action act = () => new Dentist("Valid Name", email: null!);
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]

    public void Constructor_ValidParameters_NoException()
    {
        var email = new Email("valid@example.com");
        Action act = () => new Dentist("Valid Name", email);
        act.Should().NotThrow<BusinessRuleException>();
    }
}
