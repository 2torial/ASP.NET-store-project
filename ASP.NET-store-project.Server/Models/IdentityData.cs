namespace ASP.NET_store_project.Server.Models
{
    public class IdentityData
    {
        public const string AnonymousUserPolicyName = "Anonymous"; // default

        public const string AdminUserClaimName = "admin";

        public const string AdminUserPolicyName = "Admin";

        public const string RegularUserClaimName = "user";

        public const string RegularUserPolicyName = "User";
    }

    public class CustomClaim(string type, string value, string valueType)
    {
        public string Type { get; set; } = type;

        public string Value { get; set; } = value;

        public string ValueType { get; set; } = valueType;
    }
}