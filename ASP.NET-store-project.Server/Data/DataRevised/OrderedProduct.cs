using ASP.NET_store_project.Server.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    
    public class OrderedProduct(Guid customerId, Guid supplierId, string supplierProductId, int quantity, decimal supplierCost)
    {
        private OrderedProduct() : this(Guid.NewGuid(), Guid.NewGuid(), "", 0, 0) { }

        public Guid Id { get; set; }

        public Guid CustomerId { get; set; } = customerId;

        public Guid SupplierId { get; set; } = supplierId;

        public string SupplierProductId { get; set; } = supplierProductId;

        public int Quantity { get; set; } = quantity;

        [Column(TypeName = "money")]
        public decimal Cost { get; set; } = supplierCost;

        [NotMapped]
        public decimal Profit { get => ProfitCalculator.Calculate(Cost, Supplier.ProfitMultiplier); }

        [NotMapped]
        public decimal Price { get => Cost + Profit; }

        public bool IsCheckedOut { get; set; } = false;



        public Supplier Supplier = null!;
    }
}