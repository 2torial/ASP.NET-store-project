using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorePageController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        private readonly ILogger<StorePageController> _logger;

        public StorePageController(ILogger<StorePageController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        private static readonly string[] Processors =
        [
            "Intel Core i3", "Intel Core i5", "Intel Core i7", "Intel Core i9",
            "Ryzen 3", "Ryzen 5", "Ryzen 7", "Ryzen 9",
        ];

        private static readonly string[] DiskTypes = ["HDD", "SSD"];

        private static readonly string[] DiskCapacities = ["512GB", "1024GB", "2048GB", "5096GB"];

        private static readonly string[] RAMMemories = ["8GB", "16GB", "32GB", "64GB"];

        private static readonly string[] Systems = ["Windows 10", "Windows 11", "Windows XP"];

        private static readonly string[] ViewModes = ["Gallery", "List"];

        [HttpPost("/refresh")]
        public StoreBundle Refresh()
        {
            
            foreach (string key in Request.Form.Keys)
            {
                _logger.LogError(Request.Form[key]);
            }
            
            var rng = new Random();

            

            return new StoreBundle
            {
                Settings = new StoreSettings
                {
                    Categories = ["Headsets", "Laptops/Notebooks/Ultrabooks", "Microphones"],
                    SelectedCategory = "Laptops/Notebooks/Ultrabooks",
                    SortingMethods = [
                        "Name: Ascending",
                        "Name: Descending",
                        "Price: Lowest first",
                        "Price: Highest first",
                    ],
                    SelectedSortingMethod = "Price: Lowest first",
                    Pages = 4,
                    SelectedPage = 2,
                    ViewModes = ViewModes,
                    ViewModeIcons = ["https://placehold.co/20/png", "https://placehold.co/20/png"],
                    SelectedViewMode = ViewModes[0]
                },
                Filters = new StoreFilters
                {
                    Range = new Dictionary<string, int> { { "from", 0 }, { "to", 1000 } },
                    Specifications = new Dictionary<string, string[]>
                    {
                        { "Processor", Processors },
                        { "Disk Type", DiskTypes },
                        { "Disk Capacity", DiskCapacities },
                        { "RAM Memory", RAMMemories },
                        { "System", Systems }
                    }
                },
                Items = Enumerable.Range(1, 5).Select(index => new Models.StoreItem
                {
                    Name = string.Format("Laptop #{0}", index),
                    Price = 200 * rng.Next(index, 10),
                    Images = ["https://placehold.co/150/png"],
                    Configuration = new Dictionary<string, string> {
                        { "Processor", Processors[Random.Shared.Next(Processors.Length)] },
                        { "Disk Type", DiskTypes[Random.Shared.Next(DiskTypes.Length)] },
                        { "Disk Capacity", DiskCapacities[Random.Shared.Next(DiskCapacities.Length)] },
                        { "RAM Memory", RAMMemories[Random.Shared.Next(RAMMemories.Length)] },
                        { "System", Systems[Random.Shared.Next(Systems.Length)] }
                    },
                    PageLink = index.ToString()
                }).ToArray()
           };
        }
    }
}
