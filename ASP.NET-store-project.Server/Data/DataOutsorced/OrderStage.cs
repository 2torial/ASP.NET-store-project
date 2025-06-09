namespace ASP.NET_store_project.Server.Data.DataOutsorced;

using System.ComponentModel.DataAnnotations;

// Supplier's database table
public class Stage(string type)
{
    [Key]
    public string Type { get; set; } = type;
    // Created, Pending, Finalized, Canceled

}

// Supplier's database table
public class OrderStage(Guid orderId, string stageId)
{
    public Guid OrderId { get; set; } = orderId;

    public string StageId { get; set; } = stageId;


    public Stage Stage { get; set; } = null!;

    public DateTime DateOfCreation { get; set; }

}
