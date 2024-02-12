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

        [HttpGet("/api/admin/items")]
        public IActionResult GetItems()
        {
            return Ok(new ItemListComponentData()
            {
                Items = context.Items
                    .Select(item => new StoreItems.Item
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Gallery = item.Gallery
                            .Select(image => image.Content)
                            .ToList(),
                        Specification = item.Configurations
                            .Select(config => new StoreItems.Item.Configuration
                            {
                                Label = config.Label,
                                Parameter = config.Parameter,
                            })
                            .ToList(),
                        PageLink = item.Page,
                    }).ToList(),
            });
        }
    }
}
