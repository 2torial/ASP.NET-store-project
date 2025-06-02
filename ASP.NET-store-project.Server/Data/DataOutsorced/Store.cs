namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    // Supplier's database table model
    public class Store(string name)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = name;

    }
}
