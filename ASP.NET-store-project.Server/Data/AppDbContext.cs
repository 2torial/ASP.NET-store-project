using ASP.NET_store_project.Server.Data.DataOutsorced;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace ASP.NET_store_project.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
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

            modelBuilder.Entity<Stage>()
                .ToTable("Stage");

            modelBuilder.Entity<Order>()
                .ToTable("Order");
            modelBuilder.Entity<Order>()
                .HasMany<Item>()
                .WithMany()
                .UsingEntity<ItemOrder>();
            modelBuilder.Entity<Order>()
                .HasMany<Stage>()
                .WithMany()
                .UsingEntity<OrderStage>();

            modelBuilder.Entity<OrderStage>()
                .Property(e => e.DateOfCreation).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<OrderStage>()
                .Property(e => e.TimeOfCreation).HasDefaultValueSql("NOW()");

            User[] users = [
                new("user", new SimplePasswordHasher().HashPassword("user")),
                new("root", new SimplePasswordHasher().HashPassword("root"), true)];
            modelBuilder.Entity<User>().HasData(users);

            string[] supplierKeys = ["[A]", "[B]", "[C]"];
            Supplier[] suppliers = [.. supplierKeys
                .Select(key => new Supplier(
                    $"Supplier{key[1]}", 
                    "https://localhost:5173", 
                    "filter", 
                    "select", 
                    "display",
                    "orders", 
                    "summary", 
                    "accept", 
                    "cancel"))];
            modelBuilder.Entity<Supplier>().HasData(suppliers);

            var labeledSuppliers = new Dictionary<string, Supplier>()
            {
                { supplierKeys[0], suppliers[0] },
                { supplierKeys[1], suppliers[1] },
                { supplierKeys[2], suppliers[2] }
            };

            Category[] categories = [
                new("Laptop"),
                new("Headset"),
                new("Microphone"),
                new("PersonalComputer")];
            modelBuilder.Entity<Category>().HasData(categories);

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
            Item[] items = [.. Enumerable.Range(1, 300)
                .Select(n =>
                {
                    var category = categories[rand.Next(0, categories.Length)];
                    var supplierKey = supplierKeys[rand.Next(0, 3)];
                    var supplier = labeledSuppliers[supplierKey];
                    var random3 = "" + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)];
                    var name = $"{random3} {supplierKey} {category.Type}";
                    var computerPrices = new decimal[] { 300, 400, 450, 500, 600, 700, 800, 820, 850, 900, 1000 };
                    var otherPrices = new decimal[] { 20, 30, 50, 55, 60, 90, 100, 110, 115, 150, 190, 200 };
                    var price = category == categories[0] || category == categories[3]
                        ? computerPrices[rand.Next(0, computerPrices.Length)]
                        : category == categories[1] || category == categories[2]
                            ? otherPrices[rand.Next(0, otherPrices.Length)]
                            : 0;
                    var thumbnailLink = "https://placehold.co/150x150";
                    var content = $"This is a mockup content for {name}. There is no uniform way of creating product content implemented in this project, so for now it is stored as text.";
                    return new Item(category.Type, name, price, 10, thumbnailLink, content) { SupplierKey = supplierKey };
                })];
            modelBuilder.Entity<Item>().HasData(items);

            ItemConfiguration[] itemConfigurations = [.. items
                .Select(item => categorizedConfigurationLabels[item.CategoryId]
                    .Select(label => new ItemConfiguration(item.Id, labeledConfigurations[label][rand.Next(0, labeledConfigurations[label].Length)].Id)))
                .SelectMany(itemConfigs => itemConfigs)];
            modelBuilder.Entity<ItemConfiguration>().HasData(itemConfigurations);

            AdressDetails[] adressDetails = [
                new(users[0].Id, "Śląsk", "Bielsko-Biała", "43-300", "3 Maja", "17", "91"),
                new(users[1].Id, "Dolny Śląsk", "Wrocław", "50-383", "Fryderyka Joliot-Curie", "15")];
            modelBuilder.Entity<AdressDetails>().HasData(adressDetails);

            CustomerDetails[] customerDetails = [
                new(users[0].Id, "Bartłomiej", "Żurowski", "29 02 2024 0", "bartżur@tlen.o2"),
                new(users[1].Id, "Stanisław", "August", "03 05 1791 0", "stan3@rp.on")];
            modelBuilder.Entity<CustomerDetails>().HasData(customerDetails);

            BasketProduct[] basketProducts = [.. Enumerable.Range(1, 14)
                .Select(n => {
                    var item = items[rand.Next(0, items.Length)];
                    var user = n < 7 ? users[0] : users[1];
                    var supplier = labeledSuppliers[item.SupplierKey];
                    return new BasketProduct(item.Id.ToString(), user.Id, supplier.Id, rand.Next(1, 2));
                }) // in case of distinct representation of the same items
                .GroupBy(item => item.ProductId, (_, sameItems) => sameItems.First())];
            modelBuilder.Entity<BasketProduct>().HasData(basketProducts);

            var issuerDetails = customerDetails
                .Join(adressDetails,
                    custDetails => custDetails.UserId,
                    adrDetails => adrDetails.UserId,
                    (cd, ad) => new 
                    { 
                        CustomerId = cd.UserId.ToString(), 
                        AdresseeDetails = new AdresseeDetails(
                            cd.Name, cd.Surname, cd.PhoneNumber,  cd.Email,
                            ad.Region, ad.City, ad.PostalCode, ad.StreetName, ad.HouseNumber, ad.ApartmentNumber) 
                    });

            AdresseeDetails[] adresseeDetails = [
                issuerDetails.ElementAt(0).AdresseeDetails,
                issuerDetails.ElementAt(1).AdresseeDetails];
            modelBuilder.Entity<AdresseeDetails>().HasData(adresseeDetails);

            Order[] orders = [
                new Order(adresseeDetails[0].Id, 5, "[0]", issuerDetails.ElementAt(0).CustomerId) { SupplierKey = supplierKeys[0] },
                new Order(adresseeDetails[0].Id, 5, "[0]", issuerDetails.ElementAt(0).CustomerId) { SupplierKey = supplierKeys[1] },
                new Order(adresseeDetails[0].Id, 5, "[0]", issuerDetails.ElementAt(0).CustomerId) { SupplierKey = supplierKeys[2] },
                new Order(adresseeDetails[0].Id, 5, "[0]", issuerDetails.ElementAt(0).CustomerId) { SupplierKey = supplierKeys[2] },
                new Order(adresseeDetails[1].Id, 5, "[0]", issuerDetails.ElementAt(1).CustomerId) { SupplierKey = supplierKeys[0] },
                new Order(adresseeDetails[1].Id, 5, "[0]", issuerDetails.ElementAt(1).CustomerId) { SupplierKey = supplierKeys[1] }];
            modelBuilder.Entity<Order>().HasData(orders);

            var itemsA = items.Where(item => item.SupplierKey == supplierKeys[0]);
            var itemsB = items.Where(item => item.SupplierKey == supplierKeys[1]);
            var itemsC = items.Where(item => item.SupplierKey == supplierKeys[2]);

            ItemOrder[] itemOrders = [.. orders
                .Select(order => {
                    var supplierItems = order.SupplierKey switch
                    {
                        "[A]" => itemsA,
                        "[B]" => itemsB,
                        "[C]" => itemsC,
                        _ => throw new InvalidOperationException("Unknown supplier key"),
                    };
                    var usedItemIds = new List<Guid>();
                    return Enumerable.Range(1, rand.Next(2, 5)) // 2-4 items
                        .Select(_ => { // all items must be from the same supplier
                            var item = supplierItems.ElementAt(rand.Next(0, supplierItems.Count()));
                            while (usedItemIds.Contains(item.Id))
                                item = supplierItems.ElementAt(rand.Next(0, supplierItems.Count()));
                            usedItemIds.Add(item.Id);
                            return new ItemOrder(item.Id, order.Id, 200, rand.Next(1, 4), item.ThumbnailLink);
                        });
                })
                .SelectMany(e => e)];
            modelBuilder.Entity<ItemOrder>().HasData(itemOrders);

            string[] stageTypes = [
                StageOfOrder.Created.GetDisplayName(), 
                StageOfOrder.Pending.GetDisplayName(), 
                StageOfOrder.Finished.GetDisplayName(), 
                StageOfOrder.Canceled.GetDisplayName()];
            Stage[] stages = [.. stageTypes.Select(type =>  new Stage(type))];
            modelBuilder.Entity<Stage>().HasData(stages);

            OrderStage[] orderStages = [
                new OrderStage(orders[0].Id, stages[0].Type),
                new OrderStage(orders[1].Id, stages[0].Type), new OrderStage(orders[1].Id, stages[1].Type),
                new OrderStage(orders[2].Id, stages[0].Type), new OrderStage(orders[2].Id, stages[1].Type), new OrderStage(orders[2].Id, stages[3].Type),
                new OrderStage(orders[3].Id, stages[0].Type), new OrderStage(orders[3].Id, stages[1].Type), new OrderStage(orders[3].Id, stages[2].Type),
                new OrderStage(orders[4].Id, stages[0].Type), new OrderStage(orders[4].Id, stages[1].Type), new OrderStage(orders[4].Id, stages[2].Type),
                new OrderStage(orders[5].Id, stages[0].Type), new OrderStage(orders[5].Id, stages[3].Type)];
            modelBuilder.Entity<OrderStage>().HasData(orderStages);

        }

        public DbSet<Item> Items { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<BasketProduct> BasketProducts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<AdresseeDetails> AdresseeDetails { get; set; }

        public DbSet<OrderStage> OrderStages { get; set; }

    }
}
