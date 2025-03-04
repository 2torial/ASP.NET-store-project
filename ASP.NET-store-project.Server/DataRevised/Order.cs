using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_store_project.Server.DataRevised
{
    public class Order(Guid customerId, Guid supplierId, string supplierOrderId, decimal profit, decimal cost)
    {
        [Key]
        public Guid Id { get; set; }

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