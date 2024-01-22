using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemListControler : ControllerBase
    {
        private static readonly string[] Processors =
        [
            "Intel Core i3", "Intel Core i5", "Intel Core i7", "Intel Core i9",
            "Ryzen 3", "Ryzen 5", "Ryzen 7", "Ryzen 9",
        ];

        private static readonly string[] Capacities = ["8GB", "16GB", "32GB", "64GB"];

        private readonly ILogger<ItemListControler> _logger;

        public ItemListControler(ILogger<ItemListControler> logger)
        {
            _logger = logger;
        }

        [HttpGet("/itemlist")]
        public IEnumerable<StoreItem> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new StoreItem
            {
                Name = string.Format("Laptop #{0}", index),
                ImageURL = "https://placehold.co/150x150",
                Info = [Processors[Random.Shared.Next(Processors.Length)], Capacities[Random.Shared.Next(Capacities.Length)]],
                Price = index * 200
            })
            .ToArray();
        }
    }
}
