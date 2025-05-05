using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Models.ComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    public class AdminPanelController(AppDbContext context) : ControllerBase
    {
        [HttpGet("/api/admin/users")]
        public IActionResult GetUsers()
        {
            return Ok(new UserListComponentData([.. context.Users.Select(user => new UserInfo(user.UserName, user.IsAdmin))]));
        }

    }
}
