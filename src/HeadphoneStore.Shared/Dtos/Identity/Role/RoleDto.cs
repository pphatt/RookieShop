namespace HeadphoneStore.Shared.Dtos.Identity.Role;

public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;
    public string? RoleStatus { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public DateTimeOffset? UpdatedDateTime { get; set; }
}
