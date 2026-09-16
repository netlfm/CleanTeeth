using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class TimeIntervalTests
{
    [TestMethod]
    public void Constructor_StartIsAfterEnd_Throws()
    {
        Action act = () => new TimeInterval(DateTime.UtcNow, DateTime.UtcNow.AddDays(-1));
        act.Should().Throw<BusinessRuleException>();
    }

    [TestMethod]
    public void Constructor_ValidTimeInterval_NoException()
    {
        Action act = () => new TimeInterval(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        act.Should().NotThrow<BusinessRuleException>();
    }
}
