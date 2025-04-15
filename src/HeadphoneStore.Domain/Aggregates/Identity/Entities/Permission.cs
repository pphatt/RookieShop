using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Identity.Entities;

public class Permission : Entity<long>
{
    public required Guid RoleId { get; set; }
    public required string Command { get; set; }
    public required string Function { get; set; }

    public virtual AppRole? Role { get; private set; }

    private Permission() { }

    public Permission(Guid roleId, string function, string command)
    {
        if (string.IsNullOrWhiteSpace(function))
            throw new ArgumentException("Function cannot be null or empty", nameof(function));

        if (string.IsNullOrWhiteSpace(command))
            throw new ArgumentException("Command cannot be null or empty", nameof(command));

        RoleId = roleId;
        Function = function;
        Command = command;
    }

    public string GetDisplayName() => $"{Function}.{Command}";
}
