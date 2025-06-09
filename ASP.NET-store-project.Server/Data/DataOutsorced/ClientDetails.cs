namespace ASP.NET_store_project.Server.Data.DataOutsorced;

// Supplier's database table
public class ClientDetails(Guid storeId, string clientExternalId)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StoreId { get; set; } = storeId;

    public string ClientExternalId { get; set; } = clientExternalId;


    public Store Store { get; set; } = null!;

    public List<Order> Orders { get; } = [];

}
