namespace HeadphoneStore.Contract.Dtos.Identity.Role;

public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;
}
