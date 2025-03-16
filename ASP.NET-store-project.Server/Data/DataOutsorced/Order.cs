namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Order(Guid sharedOrderId, Guid itemId, int quantity, Guid adresseeDetailsId)
    {
        public Guid Id { get; set; }

        public Guid SharedOrderId { get; set; } = sharedOrderId;

        public Guid ItemId { get; set; } = itemId;

        public int Quantity { get; set; } = quantity;

        public Guid AdresseeDetailsId { get; set; } = adresseeDetailsId;

    }
}