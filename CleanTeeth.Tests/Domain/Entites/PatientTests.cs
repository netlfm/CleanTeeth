using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class PatientTests
{
    [TestMethod]
    public void Constructor_NullName_Throws()
    {
        Action act = () => new Patient(null!, email: null!);
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Constructor_NullEmail_Throws()
    {
        Action act = () => new Patient("Valid Name", email: null!);
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Constructor_ValidParameters_NoException()
    {
        var email = new Email("valid@example.com");
        Action act = () => new Patient("Valid Name", email);
        act.Should().NotThrow<BusinessRuleException>();
    }
}
