using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataOutsorced;
using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace ASP.NET_store_project.Server.Controllers.SupplierControllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalSupplierController(AppDbContext context) : ControllerBase
    {
        [HttpGet("/api/supplier/{supplierKey}/filter/{category}")] // supplierKey is not part of an API, it is used solely for this project to differentiate "virtual" suppliers
        public IActionResult CategorizedProducts([FromRoute] ProductCategory category, [FromRoute] string supplierKey)
        {
            // Category filtering
            var categorizedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey) // Technically it's not a part of this API, that is a trick to keep "external APIs" localy
                .Where(item => item.Category.Type == category.GetDisplayName())
                .Where(item => !item.IsAvaliable)
                .Where(item => item.Configurations.Count != 0)
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Select(item => new ProductInfo() 
                {
                    Id = item.Id.ToString(),
                    Name = item.Name, 
                    Price = item.Price,
                    Tags = item.Configurations.Select(config => new ProductTag(config.Label, config.Parameter, config.Order))
                });

            return Ok(categorizedProducts);
        }

        [HttpPost("/api/supplier/{supplierKey}/select")]
        public IActionResult SelectedProducts([FromBody] IEnumerable<ProductInfo> selectedProductInfos, [FromRoute] string supplierKey)
        {
            var selectedIds = selectedProductInfos
                .Select(prod => prod.Id);
                
            var selectedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey) // Technically it's not a part of this API, that is a trick to keep "external APIs" localy
                .Where(item => selectedProductInfos
                    .Select(prod => prod.Id)
                    .Contains(item.Id.ToString()))
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Join(selectedProductInfos,
                    localProd => localProd.Id.ToString(),
                    requestProd => requestProd.Id,
                    (localProd, requestProd) => new ProductInfo()
                    {
                        Id = requestProd.Id,
                        Name = localProd.Name,
                        Price = localProd.Price,
                        Quantity = requestProd.Quantity <= localProd.Quantity
                            ? requestProd.Quantity 
                            : localProd.Quantity,
                        Gallery = [],
                        Tags = localProd.Configurations.Select(config => new ProductTag(config.Label, config.Parameter, config.Order)),
                        PageContent = localProd.PageContent,
                    });

            return Ok(selectedProducts);
        }

        [HttpGet("/api/supplier/{supplierKey}/orders/{storeId}/{customerId}")]
        public IActionResult OrderedProducts([FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] string customerId)
        {
            var orderedProducts = context.Orders
                .Where(order => order.SupplierKey == supplierKey) // Technically it's not a part of this API, it is a trick to keep "external APIs" localy
                .Where(order => order.StoreId == storeId && order.CustomerId == customerId)
                .Include(order => order.ItemOrders)
                    .ThenInclude(itemorder => itemorder.Item)
                .Include(order => order.AdresseeDetails)
                .Include(order => order.Stages)
                .AsSplitQuery()
                .Select(order => new { order.Id, order.ItemOrders, order.AdresseeDetails, order.Stages })
                .AsEnumerable()
                .Select(order => new OrderInfo(
                    order.Id.ToString(),
                    order.ItemOrders.Select(itemorder => new ProductInfo
                    {
                        Id = itemorder.Item.Id.ToString(),
                        Name = itemorder.Item.Name,
                        Price = itemorder.StorePrice,
                        Quantity = itemorder.Quantity,
                    }),
                    new CustomerInfo(
                        order.AdresseeDetails.Name, 
                        order.AdresseeDetails.Surname, 
                        order.AdresseeDetails.PhoneNumber, 
                        order.AdresseeDetails.Email),
                    new AdressInfo(
                        order.AdresseeDetails.Region,
                        order.AdresseeDetails.City,
                        order.AdresseeDetails.PostalCode,
                        order.AdresseeDetails.StreetName,
                        order.AdresseeDetails.HouseNumber,
                        order.AdresseeDetails.ApartmentNumber),
                    order.Stages.LastOrDefault()?.ToString() ?? "Unknown"));

            return Ok(orderedProducts);
        }

        [HttpPost("/api/supplier/{supplierKey}/accept/{storeId}/{customerId}")]
        public IActionResult AcceptOrder([FromBody] OrderInfo orderInfo, [FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] string customerId)
        {
            var summaries = orderInfo.Products.Select(prod => new { prod.Id, prod.Quantity });

            var viableItems = context.Items
                .Where(item => summaries.Any(summary => summary.Id == item.Id.ToString()))
                .Where(item => summaries
                    .First(summary => summary.Id == item.Id.ToString())
                    .Quantity <= item.Quantity)
                .AsEnumerable();

            if (viableItems.Count() < summaries.Count())
                return BadRequest("Not all products are aviable for sell");

            AdresseeDetails adresseeDetails = new(
                orderInfo.CustomerDetails.Name,
                orderInfo.CustomerDetails.Surname,
                orderInfo.CustomerDetails.PhoneNumber,
                orderInfo.CustomerDetails.Email,
                orderInfo.AdressDetails.Region,
                orderInfo.AdressDetails.City,
                orderInfo.AdressDetails.PostalCode,
                orderInfo.AdressDetails.StreetName,
                orderInfo.AdressDetails.HouseNumber,
                orderInfo.AdressDetails.ApartmentNumber);
            context.AdresseeDetails.Add(adresseeDetails);

            Order order = new(adresseeDetails.Id, storeId, customerId) { SupplierKey = supplierKey };
            context.Orders.Add(order);

            OrderStage orderStage = new(order.Id, StageOfOrder.Created.GetDisplayName());
            context.OrderStages.Add(orderStage);

            context.SaveChanges();

            return Ok(order.Id);
        }

        [HttpPost("/api/supplier/{supplierKey}/cancel/{storeId}/{orderId}")]
        public IActionResult CancelOrder([FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] Guid orderId)
        {
            if (!context.Orders.Any(order => order.SupplierKey == supplierKey && order.StoreId == storeId))
                return BadRequest("Invalid credentials");

            OrderStage orderStage = new(orderId, StageOfOrder.Canceled.GetDisplayName());
            context.OrderStages.Add(orderStage);
            context.SaveChanges();
            return Ok("Order canceled successfuly");
        }

    }
}
