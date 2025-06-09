namespace ASP.NET_store_project.Server.Controllers;

using Data;
using Models;
using Models.ComponentData;
using Models.StructuredData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Requires admin privileges
[ApiController]
[Route("[controller]")]
[Authorize(Policy = IdentityData.AdminUserPolicyName)]
public class AdminPanelController(AppDbContext context) : ControllerBase
{
    // Collects user data
    [HttpGet("/api/admin/users")]
    public IActionResult GetUsers() => 
        Ok(new UserListComponentData([.. context.Users.Select(user => new UserInfo(user.UserName, user.IsAdmin))]));

}
