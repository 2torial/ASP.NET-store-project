namespace ASP.NET_store_project.Server.Data;

using DataOutsorced;
using DataRevised;
using Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Configures database data not discovered by EF convention
    // Populates the database with basic semi-randomized data
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

        modelBuilder.Entity<ItemOrder>()
            .ToTable("ItemOrder");

        modelBuilder.Entity<OrderStage>()
            .ToTable("OrderStage");
        modelBuilder.Entity<OrderStage>()
            .Property(e => e.DateOfCreation).HasDefaultValueSql("NOW()");

        modelBuilder.Entity<Store>()
            .ToTable("Store");

        PasswordHasher<User> hasher = new();
        User[] users = [
            new("user", "user"),
            new("root", "root", true)];
        foreach (var user in users)
            user.PassWord = hasher.HashPassword(user, user.PassWord);
        modelBuilder.Entity<User>().HasData(users);

        Store[] stores = [new("Store")];
        modelBuilder.Entity<Store>().HasData(stores);

        ClientDetails[] clientDetails = [
            new(stores[0].Id, users[0].Id.ToString()),
            new(stores[0].Id, users[1].Id.ToString())
        ];
        modelBuilder.Entity<ClientDetails>().HasData(clientDetails);

        string[] supplierKeys = ["[A]", "[B]", "[C]"];
        Supplier[] suppliers = [.. supplierKeys
            .Select(key => new Supplier(
                $"Supplier{key[1]}", 
                "https://localhost:5173",
                stores[0].Id.ToString(),
                "filter", 
                "select", 
                "display",
                "orders", 
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
            new(ProductCategory.Laptop.GetDisplayName()),
            new(ProductCategory.Headset.GetDisplayName()),
            new(ProductCategory.Microphone.GetDisplayName()),
            new(ProductCategory.PersonalComputer.GetDisplayName())];
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
                new("System", "Windoors 10", 1),
                new("System", "Windoors 11", 2),
                new("System", "McInToss OS", 11),
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
                new("Processor", "Mackerel Core i3", 1003),
                new("Processor", "Mackerel Core i5", 1005),
                new("Processor", "Mackerel Core i7", 1007),
                new("Processor", "Mackerel Core i9", 1009),
                new("Processor", "Resin 3", 2003),
                new("Processor", "Resin 5", 2005),
                new("Processor", "Resin 7", 2007),
                new("Processor", "Resin 9", 2009),
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
        string[] companies = ["Panatonik", "Soni", "PH", "Levono", "Pineapple", "Gell", "Twinsung"];
        string[] laptopTypes = ["Laptop", "Notebook", "Ultrabook"];
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string modelChars = "abcpXYZ ";
        Item[] items = [.. Enumerable.Range(1, 300)
            .Select(n =>
            {
                var category = categories[rand.Next(0, categories.Length)];
                var supplierKey = supplierKeys[rand.Next(0, 3)];
                var supplier = labeledSuppliers[supplierKey];
                var company = companies[rand.Next(companies.Length)];
                var type = category.Type switch {
                    "Microphone" or "Headset" => category.Type,
                    "PersonalComputer" => "Computer",
                    "Laptop" => laptopTypes[rand.Next(laptopTypes.Length)],
                    _ => throw new ArgumentOutOfRangeException(category.Type)
                };
                var model = $"{chars[rand.Next(chars.Length)]}{1000 * rand.Next(1, 10) + 10 * rand.Next(10)}{modelChars[rand.Next(modelChars.Length)]}";
                var name = $"{type} {company} {model}";
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

        BasketProduct[] basketProducts = [.. Enumerable.Range(1, 14)
            .Select(n => {
                var item = items[rand.Next(0, items.Length)];
                var user = n < 7 ? users[0] : users[1];
                var supplier = labeledSuppliers[item.SupplierKey];
                return new BasketProduct(item.Id.ToString(), user.Id, supplier.Id, rand.Next(1, 2));
            }) // in case of distinct representation of the same items
            .GroupBy(item => item.ProductId, (_, sameItems) => sameItems.First())];
        modelBuilder.Entity<BasketProduct>().HasData(basketProducts);

        AdressDetails[] adressDetails = [
            new("Śląsk", "Bielsko-Biała", "43-300", "3 Maja", "17", "91"),
            new("Dolny Śląsk", "Wrocław", "50-383", "Fryderyka Joliot-Curie", "15")];
        modelBuilder.Entity<AdressDetails>().HasData(adressDetails);

        ContactDetails[] customerDetails = [
            new("Bartłomiej", "Żurowski", "29 02 2024 0", "bartżur@tlen.o2"),
            new("Stanisław", "August", "03 05 1791 0", "stan3@rp.on")];
        modelBuilder.Entity<ContactDetails>().HasData(customerDetails);

        OrderDeliveryMethod[] deliveryMethods = [
            new(DeliveryMethod.Standard.GetDisplayName()),
            new(DeliveryMethod.Express.GetDisplayName())];
        modelBuilder.Entity<OrderDeliveryMethod>().HasData(deliveryMethods);

        Order[] orders = [
            new Order(customerDetails[0].Id, adressDetails[0].Id, clientDetails[0].Id, 5, deliveryMethods[0].Type) { SupplierKey = supplierKeys[0] },
            new Order(customerDetails[0].Id, adressDetails[0].Id, clientDetails[0].Id, 5, deliveryMethods[0].Type) { SupplierKey = supplierKeys[1] },
            new Order(customerDetails[0].Id, adressDetails[0].Id, clientDetails[0].Id, 5, deliveryMethods[1].Type) { SupplierKey = supplierKeys[2] },
            new Order(customerDetails[0].Id, adressDetails[0].Id, clientDetails[0].Id, 5, deliveryMethods[0].Type) { SupplierKey = supplierKeys[2] },
            new Order(customerDetails[1].Id, adressDetails[1].Id, clientDetails[1].Id, 5, deliveryMethods[1].Type) { SupplierKey = supplierKeys[0] },
            new Order(customerDetails[1].Id, adressDetails[1].Id, clientDetails[1].Id, 5, deliveryMethods[1].Type) { SupplierKey = supplierKeys[1] }];
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

        var now = DateTime.UtcNow;
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

    public DbSet<Order> Orders { get; set; }

    public DbSet<ClientDetails> ClientDetails { get; set; }

    public DbSet<Store> Stores { get; set; }

}
