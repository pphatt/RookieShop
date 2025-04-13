using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Entities.Content;

public class UserAddress : Entity<Guid>
{
    public string Address { get; private set; }
    public string Street { get; private set; }
    public string Province { get; private set; }
    public string PhoneNumber { get; private set; }

    private UserAddress() { } // For EF Core

    public UserAddress(string street, string province, string phoneNumber) : base(Guid.NewGuid())
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Province = province ?? throw new ArgumentNullException(nameof(province));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(string street, string province, string phoneNumber)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Province = province ?? throw new ArgumentNullException(nameof(province));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        ModifiedOnUtc = DateTime.UtcNow;
    }
}
