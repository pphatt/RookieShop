namespace HeadphoneStore.Persistence.DependencyInjection.Extensions;

public static class StringExtension
{
    public static string ToFunctionPermissions(this string name)
    {
        return $"Permissions.Function.{name}";
    }

    public static string ToCommandPermissions(this string name)
    {
        return $"Permissions.Command.{name}";
    }
}
