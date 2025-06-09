namespace ASP.NET_store_project.Server.Data.DataOutsorced;

using System.ComponentModel.DataAnnotations;

// Supplier's database table
public class OrderDeliveryMethod(string type)
{
    [Key]
    public string Type { get; set; } = type;
    // Standard, Express

}
