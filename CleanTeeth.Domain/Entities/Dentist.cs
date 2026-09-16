using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class Dentist : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Gender Gender { get; private set; }
    public PhoneNumber Phone { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string? LicenseNumber { get; private set; }
    public string? Specialty { get; private set; }
    public DentistStatus Status { get; private set; }
    public bool IsDeleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    private Dentist()
    {

    }

    public Dentist(string name, Gender gender, PhoneNumber phone, Email email, string? licenseNumber, string? specialty)
    {
        EnforceBusinessRules(name, phone, email);
        Id = Guid.CreateVersion7();
        Name = name.Trim();
        Phone = phone;
        Gender = gender;
        Email = email;
        LicenseNumber = TrimToNull(licenseNumber);
        Specialty = TrimToNull(specialty);
        Status = DentistStatus.Active;
    }

    public void Update(string name, Gender gender, PhoneNumber phone, Email email, string? licenseNumber, string? specialty)
    {
        EnforceBusinessRules(name, phone, email);
        Name = name.Trim();
        Gender = gender;
        Phone = phone;
        Email = email;
        LicenseNumber = TrimToNull(licenseNumber);
        Specialty = TrimToNull(specialty);
    }
    public void ChangeStatus(DentistStatus status)
    {
        if (Status == status)
        {
            return;
        }
        Status = status;
    }
    private static void EnforceBusinessRules(string name, PhoneNumber phone, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($"The {nameof(name)} is required.");
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
    private static string? TrimToNull(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        else
        {
            return value.Trim();
        }
    }
}
