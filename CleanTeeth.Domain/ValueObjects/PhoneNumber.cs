using CleanTeeth.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace CleanTeeth.Domain.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new Regex(
        @"^\+?\d{8,15}$",
        RegexOptions.Compiled);
    private PhoneNumber()
    {

    }
    public string Value { get; } = null!;
    public PhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new BusinessRuleException($"The {nameof(phone)} is required.");
        }
        var normalized = phone.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("/", "")
            .Replace("(", "")
            .Replace(")", "");
        if (!PhoneRegex.IsMatch(normalized))
        {
            throw new BusinessRuleException($"The {nameof(phone)} is invalid.");
        }
        Value = normalized;
    }
}
