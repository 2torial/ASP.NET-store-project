using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Stage(string type)
    {
        [Key]
        public string Type { get; set; } = type;
        // Created, Pending, Finalized, Canceled

    }

    public class OrderStage(Guid orderId, string stageId)
    {
        public Guid OrderId { get; set; } = orderId;

        public string StageId { get; set; } = stageId;


        public Stage Stage { get; set; } = null!;

        public DateOnly DateOfCreation { get; set; }

        public TimeOnly TimeOfCreation { get; set; }
    }

}