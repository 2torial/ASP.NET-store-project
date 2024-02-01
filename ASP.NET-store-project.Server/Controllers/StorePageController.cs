using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorePageController : ControllerBase
    {

        private readonly AppDbContext _context;

        private readonly ILogger<StorePageController> _logger;

        public StorePageController(ILogger<StorePageController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("/refresh")]
        public StoreBundle Refresh()
        {
            foreach (string key in Request.Form.Keys)
            {
                _logger.LogInformation(key + ": " + Request.Form[key]);
                if (Request.Form[key].GetType().IsArray)
                    _logger.LogInformation("array");
            }

            return new StoreBundle(Request.Form, _context);
        }
    }
}
