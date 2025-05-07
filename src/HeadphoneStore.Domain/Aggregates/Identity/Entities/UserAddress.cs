using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Identity.Entities;

public class UserAddress : Entity<Guid>
{
    public string Address { get; set; }
    public string Street { get; set; }
    public string Province { get; set; }
    public string PhoneNumber { get; set; }

    public UserAddress() { } // For EF Core

    public UserAddress(string street, string province, string phoneNumber) : base(Guid.NewGuid())
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Province = province ?? throw new ArgumentNullException(nameof(province));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        CreatedDateTime = DateTime.UtcNow;
    }

    public void Update(string street, string province, string phoneNumber)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Province = province ?? throw new ArgumentNullException(nameof(province));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        UpdatedDateTime = DateTime.UtcNow;
    }
}
