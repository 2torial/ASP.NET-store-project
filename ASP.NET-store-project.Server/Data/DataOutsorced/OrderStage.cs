using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class OrderStage(string stage, Guid orderId)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Stage { get; set; } = stage;
        // Created, Pending, Finalized, Canceled

        [Column(TypeName = "Date")]
        public DateTime DateOfModification { get; set; } = DateTime.Now;

        public Guid OrderId { get; set; } = orderId;

    }
}