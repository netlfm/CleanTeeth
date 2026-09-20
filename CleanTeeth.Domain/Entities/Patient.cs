using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class Patient : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public string PatientNumber { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public PhoneNumber Phone { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Patient()
    {

    }
    public Patient(string name, string patientnumber, DateOnly dateofbirth, Gender gender, PhoneNumber phone, Email email, string address)
    {
        EnforceBusinessRules(name, dateofbirth, phone, email, address);
        if (string.IsNullOrWhiteSpace(patientnumber))
        {
            throw new BusinessRuleException(
                $"The {nameof(patientnumber)} is required.");
        }
        Id = Guid.CreateVersion7();
        Name = name;
        PatientNumber = patientnumber;
        DateOfBirth = dateofbirth;
        Gender = gender;
        Phone = phone;
        Email = email;
        Address = address;
    }
    public void Update(string name,DateOnly dateofbirth, Gender gender, PhoneNumber phone, Email email, string address)
    {
        EnforceBusinessRules(name, dateofbirth, phone, email, address);
        Name = name;
        DateOfBirth = dateofbirth;
        Gender = gender;
        Phone = phone;
        Email = email;
        Address = address;
    }
    public void Delete(string deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedAt = DateTime.UtcNow;
    }
    private static void EnforceBusinessRules(
        string name,
        DateOnly dateofbirth,
        PhoneNumber phone,
        Email email,
        string address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($"The {nameof(name)} is required.");
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new BusinessRuleException($"The {nameof(address)} is required.");
        }

        if (dateofbirth == default)
        {
            throw new BusinessRuleException($"The {nameof(dateofbirth)} is required.");
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
