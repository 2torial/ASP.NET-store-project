using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataOutsorced;
using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace ASP.NET_store_project.Server.Controllers.SupplierControllers
{
    // supplierKey used in code below is a way to differentiate suppliers and their individual "databases" from eachother while keeping everything localy under the same database
    // Request adresses would differ from API to API but here those are uniform, despite that all adresses are stored separately in Supplier table to imitate reality
    [ApiController]
    [Route("[controller]")]
    public class ExternalSupplierController(AppDbContext context) : ControllerBase
    {
        // Returns all products of given category with just enough information to filter them further
        [HttpGet("/api/supplier/{supplierKey}/filter/{category}")]
        public IActionResult CategorizedProducts([FromRoute] ProductCategory category, [FromRoute] string supplierKey)
        {
            // Retrieves avaliable products of selected category with at least one tag included with some other data and returns it
            var categorizedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey)
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

        // Returns products selected by their ID with extended information compared to CategorizedProducts()
        [HttpPost("/api/supplier/{supplierKey}/select")]
        public IActionResult SelectedProducts([FromBody] IEnumerable<ProductInfo> selectedProductInfos, [FromRoute] string supplierKey)
        {
            // Retrieves IDs from recieved ProductInfos
            var selectedIds = selectedProductInfos
                .Select(prod => prod.Id);
                
            // Filters products by selected IDs then joins what's left with recieved ProductInfos
            // Some information is taken from recieved data, some from database
            // If demanded quantity is not avaliable, it is set to maximum avaliable amount
            var selectedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey)
                .Where(item => selectedProductInfos
                    .Select(prod => prod.Id)
                    .Contains(item.Id.ToString()))
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Join(selectedProductInfos,
                    localProd => localProd.Id.ToString(),
                    requestProd => requestProd.Id,
                    (localProd, requestProd) => requestProd with
                    {
                        Name = localProd.Name,
                        Price = localProd.Price,
                        Quantity = requestProd.Quantity <= localProd.Quantity
                            ? requestProd.Quantity 
                            : localProd.Quantity,
                        Thumbnail = localProd.ThumbnailLink,
                        Tags = localProd.Configurations.Select(config => new ProductTag(config.Label, config.Parameter, config.Order)),
                    });

            return Ok(selectedProducts);
        }

        // Returns past orders from specific client
        [HttpGet("/api/supplier/{supplierKey}/orders/{storeId}/{customerId}")]
        public IActionResult OrderedProducts([FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] string customerId)
        {
            // Identify client
            var client = context.ClientDetails
                .Include(details => details.Orders)
                    .ThenInclude(order => order.ItemOrders)
                        .ThenInclude(itemorder => itemorder.Item)
                .Include(details => details.Orders)
                    .ThenInclude(order => order.AdressDetails)
                .Include(details => details.Orders)
                    .ThenInclude(order => order.ContactDetails)
                .Include(details => details.Orders)
                    .ThenInclude(order => order.DeliveryMethod)
                .Include(details => details.Orders)
                    .ThenInclude(order => order.OrderStages)
                        .ThenInclude(orderStage => orderStage.Stage)
                .AsSplitQuery()
                .SingleOrDefault(details => details.ClientExternalId == customerId && details.StoreId.ToString() == storeId);
            if (client == null)
                return BadRequest("Unknown client or store ID.");

            // Find past orders and return it as list-type of OrderInfo
            var orderedProducts = client.Orders
                .Where(order => order.SupplierKey == supplierKey) // Technically it's not a part of this API, it is a trick to keep "external APIs" localy
                .Select(order => new { order.Id, order.ItemOrders, order.ContactDetails, order.AdressDetails, order.OrderStages, order.DeliveryCost, order.DeliveryMethod })
                .AsEnumerable()
                .Select(order => new OrderInfo(
                    order.Id.ToString(),
                    null,
                    null,
                    -1,
                    order.DeliveryCost,
                    order.DeliveryMethod.Type,
                    order.ItemOrders.Select(itemorder => new ProductInfo
                    {
                        Id = itemorder.Item.Id.ToString(),
                        Name = itemorder.Item.Name,
                        Price = itemorder.StorePrice,
                        Quantity = itemorder.Quantity,
                        Thumbnail = itemorder.ThumbnailLink
                    }),
                    new CustomerInfo(
                        order.ContactDetails.Name,
                        order.ContactDetails.Surname,
                        order.ContactDetails.PhoneNumber,
                        order.ContactDetails.Email),
                    new AdressInfo(
                        order.AdressDetails.Region,
                        order.AdressDetails.City,
                        order.AdressDetails.PostalCode,
                        order.AdressDetails.StreetName,
                        order.AdressDetails.HouseNumber,
                        order.AdressDetails.ApartmentNumber),
                    [.. order.OrderStages.Select(orderStage => new OrderStageInfo(
                        orderStage.Stage.Type, 
                        orderStage.DateOfCreation.ToString("g"),
                        orderStage.DateOfCreation.ToString("yyyy-MM-ddTHH:mm:ss:fff")))]));

            return Ok(orderedProducts);
        }

        // Creates order in the database
        [HttpPost("/api/supplier/{supplierKey}/accept/{storeId}/{customerId}")]
        public IActionResult AcceptOrder([FromBody] OrderInfo orderInfo, [FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] string customerId)
        {
            // Identify store which requests an order
            var store = context.Stores
                .SingleOrDefault(store => store.Id.ToString() == storeId);
            if (store == null)
                return BadRequest("Given store ID is absent from the database");

            // Identify ordered products' data
            var summaries = orderInfo.Products.Select(prod => new { Id = Guid.Parse(prod.Id ?? Guid.Empty.ToString()), prod.Quantity, prod.Price });
            var orderedIds = summaries.Select(summary => summary.Id);
            var items = context.Items
                .Where(item => item.SupplierKey == supplierKey)
                .Where(item => orderedIds.Contains(item.Id))
                .AsEnumerable();

            // Add contact details based on recieved data (not saved)
            ContactDetails contactDetails = new(
                orderInfo.CustomerDetails.Name,
                orderInfo.CustomerDetails.Surname,
                orderInfo.CustomerDetails.PhoneNumber,
                orderInfo.CustomerDetails.Email);
            context.Add(contactDetails);

            // Add adress details based on recieved data (not saved)
            AdressDetails adressDetails = new(
                orderInfo.AdressDetails.Region,
                orderInfo.AdressDetails.City,
                orderInfo.AdressDetails.PostalCode,
                orderInfo.AdressDetails.StreetName,
                orderInfo.AdressDetails.HouseNumber,
                orderInfo.AdressDetails.ApartmentNumber);
            context.Add(adressDetails);

            // Identify client or create new client record (not saved)
            ClientDetails? client = context.ClientDetails
                .FirstOrDefault(client => client.ClientExternalId == customerId && client.StoreId == store.Id);
            if (client == null)
            {
                client = new(store.Id, customerId);
                context.Add(client);
            }

            // Create order (not saved)
            Order order = new(contactDetails.Id, adressDetails.Id, client.Id, orderInfo.DeliveryCost, orderInfo.DeliveryMethod) { SupplierKey = supplierKey };
            context.Add(order);

            // Add items to the order (not saved), if quantity is not met, set it to -1
            ItemOrder[] itemOrders = [.. items.Join(summaries,
                item => item.Id,
                summary => summary.Id,
                (item, summary) => new ItemOrder(
                    item.Id, 
                    order.Id, 
                    summary.Price, 
                    summary.Quantity <= item.Quantity ? summary.Quantity : -1, 
                    "https://placehold.co/150x150"))];
            context.AddRange(itemOrders);

            // Set order stage to Created (not saved)
            OrderStage orderStage = new(order.Id, StageOfOrder.Created.GetDisplayName());
            context.Add(orderStage);

            // If some of requested products are missing or have insufficient quantity – abort
            if (itemOrders.Length < summaries.Count() || itemOrders.Any(itemOrder => itemOrder.Quantity < 0))
                return BadRequest("Not all products are aviable for sell");

            // Save and return new order ID
            context.SaveChanges();

            return Ok(order.Id);
        }

        // Cancels already accepted order
        [HttpPost("/api/supplier/{supplierKey}/cancel/{storeId}/{orderId}")]
        public IActionResult CancelOrder([FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] Guid orderId)
        {
            // Abort if order can't be identified
            if (!context.Orders
                .Include(order => order.ClientDetails)
                .Any(order => order.SupplierKey == supplierKey && order.ClientDetails.StoreId.ToString() == storeId))
                    return BadRequest("Invalid credentials");

            // Change stage of the order to Canceled
            OrderStage orderStage = new(orderId, StageOfOrder.Canceled.GetDisplayName());
            context.Add(orderStage);
            context.SaveChanges();
            return Ok("Order canceled successfuly");
        }

        // Returns product's page data
        [HttpGet("/api/supplier/{supplierKey}/display/{productId}")]
        public IActionResult DisplayProduct([FromRoute] string supplierKey, [FromRoute] string productId)
        {
            var product = context.Items
                .Where(item => item.SupplierKey == supplierKey)
                .SingleOrDefault(item => productId == item.Id.ToString());

            return product == null ? NotFound() : Ok(product.PageContent);
        }
    }
}
