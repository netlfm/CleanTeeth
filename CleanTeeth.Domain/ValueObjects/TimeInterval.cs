using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.ValueObjects;

public record TimeInterval
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeInterval(DateTime start, DateTime end)
    {
        if (start >= end)
        {
            throw new BusinessRuleException($"The {nameof(start)} must be less than or equal to {nameof(end)}.");
        }
        Start = start;
        End = end;
    }
}
