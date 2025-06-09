namespace ASP.NET_store_project.Server.Controllers.SupplierControllers;

using Data;
using Data.DataOutsorced;
using Data.Enums;
using Models.StructuredData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

// Controller below uses supplierKey value to differentiate suppliers and their individual "databases" from eachother while keeping everything localy under the same database
// Request adresses would differ from API to API but here those are uniform, despite that all adresses are stored separately in Supplier table to imitate reality
[ApiController]
[Route("[controller]")]
public class ExternalSupplierController(AppDbContext context) : ControllerBase
{
    // Collects all products within requested category with just enough information required for product filtering
    [HttpGet("/api/supplier/{supplierKey}/filter/{category}")]
    public IActionResult CategorizedProducts([FromRoute] ProductCategory category, [FromRoute] string supplierKey)
    {
        // Collects avaliable products of selected category containing at least one tag 
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

    // Collects products withing selected ID range with extended information compared to CategorizedProducts()
    [HttpPost("/api/supplier/{supplierKey}/select")]
    public IActionResult SelectedProducts([FromBody] IEnumerable<ProductInfo> selectedProductInfos, [FromRoute] string supplierKey)
    {
        // Collects IDs from recieved ProductInfos
        var selectedIds = selectedProductInfos
            .Select(prod => prod.Id);
            
        // Filters products by selected IDs then joins what's left with recieved ProductInfos
        // Some information is taken from recieved data, some from the database
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

    // Collects past orders of specific client
    [HttpGet("/api/supplier/{supplierKey}/orders/{storeId}/{customerId}")]
    public IActionResult OrderedProducts([FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] string customerId)
    {
        // Collects client data
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
            .SingleOrDefault(details => details.ClientExternalId == customerId && details.StoreId == Guid.Parse(storeId));
        if (client == null)
            return BadRequest("Unknown client or store ID.");

        // Collects past orders in the format of OrderInfo
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

    // Places order in the database
    [HttpPost("/api/supplier/{supplierKey}/accept/{storeId}/{customerId}")]
    public IActionResult AcceptOrder([FromBody] OrderInfo orderInfo, [FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] string customerId)
    {
        // Identifies store which requested an order
        var store = context.Stores
            .SingleOrDefault(store => store.Id == Guid.Parse(storeId));
        if (store == null)
            return BadRequest("Given store ID is absent from the database");

        // Identifies ordered product data
        var summaries = orderInfo.Products.Select(prod => new { Id = Guid.Parse(prod.Id ?? Guid.Empty.ToString()), prod.Quantity, prod.Price });
        var orderedIds = summaries.Select(summary => summary.Id);
        var items = context.Items
            .Where(item => item.SupplierKey == supplierKey)
            .Where(item => orderedIds.Contains(item.Id))
            .AsEnumerable();

        // Adds contact details based on recieved data (postponed saving)
        ContactDetails contactDetails = new(
            orderInfo.CustomerDetails.Name,
            orderInfo.CustomerDetails.Surname,
            orderInfo.CustomerDetails.PhoneNumber,
            orderInfo.CustomerDetails.Email);
        context.Add(contactDetails);

        // Adds adress details based on recieved data (postponed saving)
        AdressDetails adressDetails = new(
            orderInfo.AdressDetails.Region,
            orderInfo.AdressDetails.City,
            orderInfo.AdressDetails.PostalCode,
            orderInfo.AdressDetails.StreetName,
            orderInfo.AdressDetails.HouseNumber,
            orderInfo.AdressDetails.ApartmentNumber);
        context.Add(adressDetails);

        // Identifies client or creates new client record (postponed saving)
        ClientDetails? client = context.ClientDetails
            .FirstOrDefault(client => client.ClientExternalId == customerId && client.StoreId == store.Id);
        if (client == null)
        {
            client = new(store.Id, customerId);
            context.Add(client);
        }

        // Creates an order (postponed saving)
        Order order = new(contactDetails.Id, adressDetails.Id, client.Id, orderInfo.DeliveryCost, orderInfo.DeliveryMethod) { SupplierKey = supplierKey };
        context.Add(order);

        // Adds items to the order (postponed saving), if quantity is not met, sets it to -1
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

        // Sets order stage to Created (postponed saving)
        OrderStage orderStage = new(order.Id, StageOfOrder.Created.GetDisplayName());
        context.Add(orderStage);

        // Confirms all requested products are avaliable
        if (itemOrders.Length < summaries.Count() || itemOrders.Any(itemOrder => itemOrder.Quantity < 0))
            return BadRequest("Not all products are aviable for sell");

        // Saves and returns placed order ID
        context.SaveChanges();

        return Ok(order.Id);
    }

    // Cancels already accepted order
    [HttpPost("/api/supplier/{supplierKey}/cancel/{storeId}/{orderId}")]
    public IActionResult CancelOrder([FromRoute] string supplierKey, [FromRoute] string storeId, [FromRoute] Guid orderId)
    {
        // Identifies order
        if (!context.Orders
            .Include(order => order.ClientDetails)
            .Any(order => order.SupplierKey == supplierKey && order.ClientDetails.StoreId == Guid.Parse(storeId)))
                return BadRequest("Invalid credentials");

        // Changes stage of the order to Canceled
        OrderStage orderStage = new(orderId, StageOfOrder.Canceled.GetDisplayName());
        context.Add(orderStage);
        context.SaveChanges();
        return Ok("Order canceled successfuly");
    }

    // Returns product's page data
    [HttpGet("/api/supplier/{supplierKey}/display/{productId}")]
    public IActionResult DisplayProduct([FromRoute] string supplierKey, [FromRoute] string productId)
    {
        // Identifies product
        var product = context.Items
            .Where(item => item.SupplierKey == supplierKey)
            .SingleOrDefault(item => productId == item.Id.ToString());

        return product == null ? NotFound() : Ok(product.PageContent);
    }
}
