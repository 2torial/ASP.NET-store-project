using ASP.NET_store_project.Server.Data.DataOutsorced;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_store_project.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemCategory>()
                .ToTable("ItemCategory");

            modelBuilder.Entity<Item>()
                .ToTable("Item");
            modelBuilder.Entity<Item>()
                .HasMany(p => p.Configurations)
                .WithMany()
                .UsingEntity<ItemConfiguration>();

            modelBuilder.Entity<User>()
                .ToTable("User");
            modelBuilder.Entity<User>()
                .Property(p => p.IsAdmin)
                .HasDefaultValue(false);

            modelBuilder.Entity<BasketProduct>()
                .ToTable("BasketProduct");

            modelBuilder.Entity<Supplier>()
                .ToTable("Supplier");

            modelBuilder.Entity<Order>()
                .HasMany(p => p.Items)
                .WithMany()
                .UsingEntity<ItemOrder>();

            modelBuilder.Entity<OrderStage>()
                .ToTable("OrderStage");

            User[] users = [
                new("user", new SimplePasswordHasher().HashPassword("user")),
                new("root", new SimplePasswordHasher().HashPassword("root"), true)];
            modelBuilder.Entity<User>().HasData(users);

            string[] supplierKeys = ["[A]", "[B]", "[C]"];
            Supplier[] suppliers = [
                new("SupplierA", "https://localhost:5173/", "filter", "select", "summary", "accept", "cancel"),
                new("SupplierB", "https://localhost:5173/", "filter", "select", "summary", "accept", "cancel"),
                new("SupplierC", "https://localhost:5173/", "filter", "select", "summary", "accept", "cancel")];
            modelBuilder.Entity<Supplier>().HasData(suppliers);

            var labeledSuppliers = new Dictionary<string, Supplier>()
            {
                { supplierKeys[0], suppliers[0] },
                { supplierKeys[1], suppliers[1] },
                { supplierKeys[2], suppliers[2] }
            };

            ItemCategory[] categories = [
                new("Laptop"),
                new("Headset"),
                new("Microphone"),
                new("PersonalComputer")];
            modelBuilder.Entity<ItemCategory>().HasData(categories);

            Dictionary<string, Configuration[]> labeledConfigurations = new()
            {
                { "RAM Memory", [
                    new("RAM Memory", "4 GB", 4),
                    new("RAM Memory", "8 GB", 8),
                    new("RAM Memory", "16 GB", 16),
                    new("RAM Memory", "32 GB", 32),
                    new("RAM Memory", "64 GB", 64),
                    new("RAM Memory", "No Memory")] },
                { "System", [
                    new("System", "Windows 10", 1),
                    new("System", "Windows 11", 2),
                    new("System", "MacOS", 11),
                    new("System", "No System")] },
                { "Disk", [
                    new("Disk", "SSD", 1),
                    new("Disk", "HDD", 2),
                    new("Disk", "No Disk")] },
                { "Disk Capacity", [
                    new("Disk Capacity", "512 GB", 512),
                    new("Disk Capacity", "1024 GB", 1024),
                    new("Disk Capacity", "2048 GB", 2048),
                    new("Disk Capacity", "4096 GB", 4096)] },
                { "Processor", [
                    new("Processor", "Intel Core i3", 1),
                    new("Processor", "Intel Core i5", 1),
                    new("Processor", "Intel Core i7", 1),
                    new("Processor", "Intel Core i9", 1),
                    new("Processor", "Ryzen 3", 2),
                    new("Processor", "Ryzen 5", 2),
                    new("Processor", "Ryzen 7", 2),
                    new("Processor", "Ryzen 9", 2),
                    new("Processor", "No Processor")] },
                { "Cord Length", [
                    new("Cord Length", "1 m", 1),
                    new("Cord Length", "2 m", 2),
                    new("Cord Length", "Wireless")] }
            };
            Dictionary<string, IEnumerable<string>> categorizedConfigurationLabels = new()
            {
                { categories[0].Type, ["RAM Memory", "System", "Disk", "Disk Capacity", "Processor"] },
                { categories[1].Type, ["Cord Length"] },
                { categories[2].Type, ["Cord Length"] },
                { categories[3].Type, ["RAM Memory", "System", "Disk", "Disk Capacity", "Processor"] }
            };
            modelBuilder.Entity<Configuration>().HasData(labeledConfigurations
                .SelectMany(kvp => kvp.Value));

            var rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Item[] items = Enumerable.Range(1, 300)
            .Select(n =>
            {
                var category = categories[rand.Next(0, categories.Length)];
                var supplierKey = supplierKeys[rand.Next(0, 3)];
                var random3 = "" + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)];
                var name = $"{random3} {supplierKey} {category.Type}";
                var computerPrices = new decimal[] { 300, 400, 450, 500, 600, 700, 800, 820, 850, 900, 1000 };
                var otherPrices = new decimal[] { 20, 30, 50, 55, 60, 90, 100, 110, 115, 150, 190, 200 };
                var price = category == categories[0] || category == categories[3]
                    ? computerPrices[rand.Next(0, computerPrices.Length)]
                    : category == categories[1] || category == categories[2]
                        ? otherPrices[rand.Next(0, otherPrices.Length)]
                        : 0;
                return new Item(category.Type, name, price, 3) { SupplierKey = supplierKey };
            }).ToArray();
            modelBuilder.Entity<Item>().HasData(items);

            Image[] images = items
                .Select(item => Enumerable.Range(1, rand.Next(1, 4))
                    .Select(_ => new Image("https://placehold.co/150x150", item.Id)))
                .SelectMany(imgs => imgs)
                .ToArray();
            modelBuilder.Entity<Image>().HasData(images);

            ItemConfiguration[] itemConfigurations = items
            .Select(item => categorizedConfigurationLabels[item.CategoryId]
                .Select(label => new ItemConfiguration(item.Id, labeledConfigurations[label][rand.Next(0, labeledConfigurations[label].Length)].Id)))
            .SelectMany(itemConfigs => itemConfigs)
            .ToArray();
            modelBuilder.Entity<ItemConfiguration>().HasData(itemConfigurations);

            modelBuilder.Entity<AdressDetails>().HasData(
                new(users[0].Id, "Śląsk", "Bielsko-Biała", "43-300", "3 Maja", "17", "91"),
                new(users[1].Id, "Dolny Śląsk", "Wrocław", "50-383", "Fryderyka Joliot-Curie", "15"));

            modelBuilder.Entity<CustomerDetails>().HasData(
                new(users[0].Id, "Bartłomiej", "Żurowski", "29 02 2024 0", "bartżur@tlen.o2"),
                new(users[1].Id, "Stanisław", "August", "03 05 1791 0", "stan3@rp.on"));

            BasketProduct[] orderedProducts = new BasketProduct[40];
            int i = 0;
            while (i < 40)
            {
                var item = items[rand.Next(0, items.Length)];
                if (orderedProducts.Any(orderedItem => orderedItem?.ProductId == item.Id.ToString()))
                    continue;
                var user = i < 28 ? users[0] : users[1];
                var supplier = labeledSuppliers[item.SupplierKey];
                orderedProducts[i] = new BasketProduct(item.Id.ToString(), user.Id, supplier.Id, rand.Next(1, 2));
                i++;
            }
            modelBuilder.Entity<BasketProduct>().HasData(orderedProducts);
        }


        public DbSet<Item> Items { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<BasketProduct> BasketProducts { get; set; }

    }
}
