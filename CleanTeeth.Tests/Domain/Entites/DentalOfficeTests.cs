using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class DentalOfficeTests
{
    [TestMethod]
    public void Constructor_NullName_Throws()
    {
        Action act = () => new DentalOffice(null!);
        act.Should().Throw<BusinessRuleException>();
    }
}
