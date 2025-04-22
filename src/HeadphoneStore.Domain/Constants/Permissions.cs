namespace HeadphoneStore.Domain.Constants;

public static partial class Permissions
{
    public static class Function
    {
        public const string USER = nameof(USER);
        public const string ROLE = nameof(ROLE);
        public const string CATEGORY = nameof(CATEGORY);
        public const string PRODUCT = nameof(PRODUCT);
        public const string BRAND = nameof(BRAND);
        public const string ORDER = nameof(ORDER);
        public const string PERMISSION = nameof(PERMISSION);
    }
}

public static partial class Permissions
{
    public static class Command
    {
        public const string CREATE = nameof(CREATE);
        public const string VIEW = nameof(VIEW);
        public const string EDIT = nameof(EDIT);
        public const string DELETE = nameof(DELETE);
    }
}
