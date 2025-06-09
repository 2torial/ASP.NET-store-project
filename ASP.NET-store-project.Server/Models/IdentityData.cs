namespace ASP.NET_store_project.Server.Models;

// Reserved names used by authentication and authorization services
public class IdentityData
{
    public const string AnonymousUserPolicyName = "Anonymous"; // default

    public const string AdminUserClaimName = "admin";

    public const string AdminUserPolicyName = "Admin";

    public const string RegularUserClaimName = "user";

    public const string RegularUserPolicyName = "User";
}
