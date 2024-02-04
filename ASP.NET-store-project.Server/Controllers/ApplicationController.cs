using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ApplicationController(
        AppDbContext context, 
        ILogger<ApplicationController> logger
    ) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        private readonly ILogger<ApplicationController> _logger = logger;

        [HttpPost("reload")]
        public StoreComponentData Reload()
        {
            return new StoreComponentData(Request.Form, _context);
        }

        //[ActionName("SignIn")]
        //public bool SignIn(HttpContext httpContext)
        //{
        //    var formData = Request.Form;
        //    var user = formData.TryGetValue("UserName", out var username)
        //        ? formData.TryGetValue("PassWord", out var password)
        //            ? context.Customers
        //                .Where(customer => customer.UserName == username.ToString())
        //                .Where(customer => customer.PassWord == password.ToString())
        //                .FirstOrDefault()
        //            : null
        //        : null;
        //    if (user == null) return false;

        //}
    }
}
