using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Application.Exceptions;

public class AppointmentOverlapException : BusinessRuleException
{
    public AppointmentOverlapException(string message)
        : base(message)
    {
    }
}
