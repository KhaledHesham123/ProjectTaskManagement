namespace ProjectTaskManagement.Domain.Common;

public static class ApplicationConstants
{
    public const string AdminId = "0EAEF81B-2E06-4404-9452-37AA39228D3E";

    public enum UserRole
    {
        Admin = 1,
        User = 2,
        SuperAdmin = 3
    }

    public static class AppPermissions
    {
        public const string ViewId = "0EAEF81B-2E06-4404-9452-37AA39228D3A";
        public const string CreateId = "0EAEF81B-2E06-4404-9452-37AA39228D39";
        public const string EditId = "0EAEF81B-2E06-4404-9452-37AA39228D38";
        public const string DeleteId = "0EAEF81B-2E06-4404-9452-37AA39228D37";

        public const string PermissionType = "Permission";

        public const string View = "View";
        public const string Create = "Create";
        public const string Edit = "Edit";
        public const string Delete = "Delete";
    }
}
