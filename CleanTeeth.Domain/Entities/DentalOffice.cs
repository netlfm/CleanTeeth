using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class DentalOffice : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public PhoneNumber Phone { get; private set; } = null!;
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private DentalOffice()
    {

    }
    public DentalOffice(string name, string address, PhoneNumber phone, Email email)
    {
        EnforceBusinessRules(name, address, phone, email);
        Id = Guid.CreateVersion7();
        Name = name.Trim();
        Address = address.Trim();
        Phone = phone;
        Email = email;
    }
    public void Update(string name, string address, PhoneNumber phone, Email email)
    {
        EnforceBusinessRules(name, address, phone, email);
        Name = name.Trim();
        Address = address.Trim();
        Phone = phone;
        Email = email;
    }
    public void Delete(string deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedAt = DateTime.UtcNow;
    }
    private static void EnforceBusinessRules(string name, string address, PhoneNumber phone, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($"The {nameof(name)} is required.");
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new BusinessRuleException($"The {nameof(address)} is required.");
        }

        if (phone is null)
        {
            throw new BusinessRuleException($"The {nameof(phone)} is required.");
        }

        if (email is null)
        {
            throw new BusinessRuleException($"The {nameof(email)} is required.");
        }
    }
}
