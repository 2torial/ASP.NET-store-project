using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ASP.NET_store_project.Server.Models.UserListComponentData;

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
            return Ok(new UserListComponentData()
            {
                Users = context.Users
                    .Select(user => new UserData
                    {
                        Name = user.UserName,
                        IsAdmin = user.IsAdmin,
                    }).ToList()
            });
        }
    }
}
