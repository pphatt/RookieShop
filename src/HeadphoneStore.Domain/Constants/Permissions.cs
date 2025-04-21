using System.ComponentModel;

namespace HeadphoneStore.Domain.Constants;

public static class Permissions
{
    public static class Dashboards
    {
        [Description("View Dashboard")]
        public const string View = "Permissions.Dashboards.View";
    }

    public static class Roles
    {
        [Description("Create New Role")]
        public const string Create = "Permissions.Roles.Create";

        [Description("View Roles")]
        public const string View = "Permissions.Roles.View";

        [Description("Update Role")]
        public const string Edit = "Permissions.Roles.Edit";

        [Description("Delete Role")]
        public const string Delete = "Permissions.Roles.Delete";
    }

    public static class Users
    {
        [Description("Create New User")]
        public const string Create = "Permissions.Users.Create";

        [Description("View Users")]
        public const string View = "Permissions.Users.View";

        [Description("Update User")]
        public const string Edit = "Permissions.Users.Edit";

        [Description("Delete User")]
        public const string Delete = "Permissions.Users.Delete";
    }

    public static class SettingGAC
    {
        [Description("Manage Guest Setting Access Control")]
        public const string Manage = "Permissions.SettingGAC.Manage";
    }

    public static class ActivityLogs
    {
        [Description("View Activity Logs")]
        public const string View = "Permissions.ActivityLogs.View";
    }
}
