using System.ComponentModel.DataAnnotations.Schema;

using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Enumeration;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Domain.Aggregates.Identity.Entities;

[Table("AppUsers")]
public class AppUser : IdentityUser<Guid>, IAuditableEntity
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTimeOffset? DayOfBirth { get; set; }
    public DateTimeOffset? LastLoginDate { get; set; }
    public string? Avatar { get; set; } = default!;
    public string? Bio { get; set; }
    public EntityStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedDateTime { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } // UserRoles
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; } // UserClaims
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; } // UserLogins
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; } // UserTokens

    private readonly List<UserAddress> _addresses = new();
    public virtual IReadOnlyCollection<UserAddress> Addresses => _addresses.AsReadOnly();

    protected AppUser() { }

    private AppUser(string email)
    {
        //if (string.IsNullOrWhiteSpace(email))
        //    throw new UsersException.FieldNullOrWhitespaceException("Email");

        Id = Guid.NewGuid();
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
        IsActive = true;
        Status = EntityStatus.Active; // set this to Active
        IsDeleted = false;
        CreatedDateTime = DateTimeOffset.UtcNow;
        UserName = Email;
        EmailConfirmed = true; // don't need to confirm
        LockoutEnabled = false; // don't need to confirm email
        NormalizedUserName = NormalizedEmail;
        SecurityStamp = Guid.NewGuid().ToString();
    }

    private AppUser(string email, string firstName, string lastName, string phoneNumber) : this(email)
    {
        //if (string.IsNullOrWhiteSpace(phoneNumber))
        //    throw new UsersException.FieldNullOrWhitespaceException("Phone Number");
        //if (string.IsNullOrWhiteSpace(firstName))
        //    throw new UsersException.FieldNullOrWhitespaceException("First name");
        //if (string.IsNullOrWhiteSpace(lastName))
        //    throw new UsersException.FieldNullOrWhitespaceException("Last name");
        //if (string.IsNullOrWhiteSpace(email))
        //    throw new UsersException.FieldNullOrWhitespaceException("Email");

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    private AppUser(string email, string firstName, string lastName, string phoneNumber, string passwordHash)
        : this(email, firstName, lastName, phoneNumber)
    {
        PasswordHash = passwordHash;
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    public bool IsActiveUser()
        => EntityStatus.Active == Status && EmailConfirmed && !string.IsNullOrWhiteSpace(PasswordHash);

    public bool CanBeResetPasswordAfterConfirmedEmail()
        => !IsActiveUser();

    public static AppUser Create(string email, string firstName, string lastName, string phoneNumber)
    {
        return new(email, firstName, lastName, phoneNumber);
    }
   
    public static AppUser Create(string email)
    {
        return new(email);
    }

    public static AppUser Create(string email, string firstName, string lastName, string phoneNumber, string password)
    {
        return new(email, firstName, lastName, phoneNumber, password);
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedDateTime = DateTimeOffset.Now;
    }

    public void Logout()
    {
        LastLoginDate = DateTimeOffset.UtcNow;
    }

    public void Update(string firstName, string lastName, string? hasedPassword = null, string? phoneNumber = null)
    {
        Update(firstName, lastName);

        if (!string.IsNullOrWhiteSpace(hasedPassword))
        {
            PasswordHash = hasedPassword;
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            PhoneNumber = phoneNumber;
        }
    }

    public void CompleteRegistration(string firstName, string lastName, string phoneNumber)
    {
        Update(firstName, lastName, phoneNumber: phoneNumber);
    }
}
