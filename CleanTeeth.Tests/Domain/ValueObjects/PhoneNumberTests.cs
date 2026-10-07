using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class PhoneNumberTests
{
    [TestMethod]
    public void Constructor_NullPhone_Throws()
    {
        Action act = () => new PhoneNumber(null!);
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_EmptyPhone_Throws()
    {
        Action act = () => new PhoneNumber(" ");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_TooShort_Throws()
    {
        Action act = () => new PhoneNumber("1234567");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_TooLong_Throws()
    {
        Action act = () => new PhoneNumber("1234567890123456");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_ContainsLetters_Throws()
    {
        Action act = () => new PhoneNumber("12345abc");
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_ValidPhone_SetsValue()
    {
        var phone = new PhoneNumber("+4912345678");
        phone.Value.Should().Be("+4912345678");
    }

    [TestMethod]
    public void Constructor_ValidPhoneWithSpaces_Normalizes()
    {
        var phone = new PhoneNumber("+49 123 456 78");
        phone.Value.Should().Be("+4912345678");
    }

    [TestMethod]
    public void Constructor_ValidPhoneWithDashesAndParens_Normalizes()
    {
        var phone = new PhoneNumber("+49 (123) 456-78");
        phone.Value.Should().Be("+4912345678");
    }
}
