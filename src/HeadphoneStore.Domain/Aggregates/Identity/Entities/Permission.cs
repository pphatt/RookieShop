using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Identity.Entities;

public class Permission : Entity<long>
{
    public string Command { get; private set; }
    public string Function { get; private set; }

    public Guid RoleId { get; private set; }
    public virtual AppRole? Role { get; private set; }

    protected Permission() { }

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

    public string GetDisplayName() => $"Permissions.{Function.Split('.')[2]}.{Command.Split('.')[2]}";
}
