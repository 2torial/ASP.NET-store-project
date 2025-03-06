using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class Order(Guid customerId, Guid supplierId, string supplierOrderId, decimal profit, decimal cost, Guid? combinedOrderId = null)
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? CombinedOrderId { get; set; } = combinedOrderId;

        public Guid CustomerId { get; set; } = customerId;

        public Guid SupplierId { get; set; } = supplierId;

        public string SupplierOrderId { get; set; } = supplierOrderId;

        [Column(TypeName = "money")]
        public decimal Profit { get; set; } = profit;

        [Column(TypeName = "money")]
        public decimal Cost { get; set; } = cost;

        public bool IsProductedReturned { get; set; } = false;



        public User Customer { get; set; } = null!;

        public Supplier Supplier { get; set; } = null!;

    }
}