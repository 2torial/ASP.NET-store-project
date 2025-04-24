namespace ASP.NET_store_project.Server.Data.DataRevised
{
    
    public class BasketProduct(Guid customerId, Guid supplierId, string supplierProductId, int quantity)
    {
        private BasketProduct() : this(Guid.NewGuid(), Guid.NewGuid(), "", 0) { }

        public Guid Id { get; set; }

        public Guid CustomerId { get; set; } = customerId;

        public Guid SupplierId { get; set; } = supplierId;

        public string SupplierProductId { get; set; } = supplierProductId;

        public int Quantity { get; set; } = quantity;

        public bool IsCheckedOut { get; set; } = false;



        public Supplier Supplier = null!;
    }
}