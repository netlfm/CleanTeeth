using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class EmailTests
{
    [TestMethod]
    public void Constructor_NullEmail_Throws()
    {
        Action act = () => new Email(null!);
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Constructor_EmailWithoutAt_Throws()
    {
        Action act = () => new Email("invalidemail.com");
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Constructor_ValidEmail_NoException()
    {
        Action act = () => new Email("valid@example.com");
        act.Should().NotThrow<BusinessRuleException>();
    }
}
