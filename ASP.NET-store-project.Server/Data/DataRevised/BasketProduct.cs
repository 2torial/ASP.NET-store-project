namespace ASP.NET_store_project.Server.Data.DataRevised;

using System.ComponentModel.DataAnnotations;

// Internal database table
public class BasketProduct(string productId, Guid customerId, Guid supplierId, int quantity)
{
    private BasketProduct() : this("", Guid.NewGuid(), Guid.NewGuid(), 0) { }

    [Key]
    public Guid DatabaseId { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; } = customerId;

    public Guid SupplierId { get; set; } = supplierId;

    public string ProductId { get; set; } = productId;

    public int Quantity { get; set; } = quantity;

    public bool IsCheckedOut { get; set; } = false;



    public Supplier Supplier { get; set; } = null!;
}
