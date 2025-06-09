namespace ASP.NET_store_project.Server.Data.DataOutsorced;

// Supplier's database table
public class Order(Guid contactDetailsId, Guid adressDetailsId, Guid clientDetailsId, decimal deliveryCost, string deliveryMethodId)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string SupplierKey { get; set; } // Not part of the API, it's essential to create "virtual suppliers" localy

    public Guid ContactDetailsId { get; set; } = contactDetailsId;

    public Guid AdressDetailsId { get; set; } = adressDetailsId;

    public Guid ClientDetailsId { get; set; } = clientDetailsId;

    public decimal DeliveryCost { get; set; } = deliveryCost;

    public string DeliveryMethodId { get; set; } = deliveryMethodId;



    public ContactDetails ContactDetails { get; set; } = null!;

    public AdressDetails AdressDetails { get; set; } = null!;

    public ClientDetails ClientDetails { get; set; } = null!;

    public OrderDeliveryMethod DeliveryMethod { get; set; } = null!;

    public List<ItemOrder> ItemOrders { get; } = [];

    public List<OrderStage> OrderStages { get; } = [];

}

// Supplier's database table
public class ItemOrder(Guid itemId, Guid orderId, decimal storePrice, int quantity, string thumbnailLink)
{
    public Guid ItemId { get; set; } = itemId;

    public Guid OrderId { get; set; } = orderId;

    public decimal StorePrice { get; set; } = storePrice;

    public int Quantity { get; set; } = quantity;

    public string ThumbnailLink { get; set; } = thumbnailLink;


    public Item Item { get; set; } = null!;

}
