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

            modelBuilder.Entity<User>()
                .Property(p => p.IsAdmin)
                .HasDefaultValue(false);

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Item>()
                .ToTable("Item");

            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<OrderedProduct>()
                .ToTable("OrderedProduct");

            modelBuilder.Entity<Supplier>()
                .ToTable("Supplier");

            modelBuilder.Entity<Item>()
                .HasMany(e => e.Configurations)
                .WithMany(e => e.Items)
                .UsingEntity<ItemConfiguration>();

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(
                new("user", new SimplePasswordHasher().HashPassword("user")) { Id = userId1 },
                new("root", new SimplePasswordHasher().HashPassword("root"), true) { Id = userId2 });

            var supplierId1 = Guid.NewGuid();
            var supplierId2 = Guid.NewGuid();
            var supplierId3 = Guid.NewGuid();

            modelBuilder.Entity<Supplier>().HasData(
                new("SupplierA","https://localhost:5173/", "filter", "select") { Id = supplierId1 },
                new("SupplierB", "https://localhost:5173/", "filter", "select", 200) { Id = supplierId2 },
                new("SupplierC", "https://localhost:5173/", "filter", "select") { Id = supplierId3 });

            modelBuilder.Entity<Category>().HasData(
                new("Laptops", "Laptops/Notebooks/Ultrabooks"),
                new("Headsets", "Headsets"),
                new("Microphones", "Microphones"));

            Guid[] configs = new Guid[28];
            for (int i = 0; i < configs.Length; i++)
                configs[i] = Guid.NewGuid();

            modelBuilder.Entity<Configuration>().HasData(
                new("RAM Memory", "4 GB", 4) { Id = configs[0] },
                new("RAM Memory", "8 GB", 8) { Id = configs[1] },
                new("RAM Memory", "16 GB", 16) { Id = configs[2] },
                new("RAM Memory", "32 GB", 32) { Id = configs[3] },
                new("RAM Memory", "64 GB", 64) { Id = configs[4] },
                new("RAM Memory", "No Memory") { Id = configs[5] },
                new("System", "Windows 10", 1) { Id = configs[6] },
                new("System", "Windows 11", 2) { Id = configs[7] },
                new("System", "MacOS", 11) { Id = configs[8] },
                new("System", "No System") { Id = configs[9] },
                new("Disk", "SSD", 1) { Id = configs[10] },
                new("Disk", "SSD", 2) { Id = configs[11] },
                new("Disk", "No Disk") { Id = configs[12] },
                new("Disk Capacity", "512 GB", 512) { Id = configs[13] },
                new("Disk Capacity", "1024 GB", 1024) { Id = configs[14] },
                new("Disk Capacity", "2048 GB", 2048) { Id = configs[15] },
                new("Disk Capacity", "4096 GB", 4096) { Id = configs[16] },
                new("Processor", "Intel Core i3", 1) { Id = configs[17] },
                new("Processor", "Intel Core i5", 1) { Id = configs[18] },
                new("Processor", "Intel Core i7", 1) { Id = configs[19] },
                new("Processor", "Intel Core i9", 1) { Id = configs[20] },
                new("Processor", "Ryzen 3", 2) { Id = configs[21] },
                new("Processor", "Ryzen 5", 2) { Id = configs[22] },
                new("Processor", "Ryzen 7", 2) { Id = configs[23] },
                new("Processor", "Ryzen 9", 2) { Id = configs[24] },
                new("Processor", "No Processor") { Id = configs[25] },
                new("Cord Length", "1 m", 1) { Id = configs[26] },
                new("Cord Length", "2 m", 2) { Id = configs[27] });

            Guid[] items = new Guid[12];
            for (int i = 0; i < items.Length; i++)
                items[i] = Guid.NewGuid();

            modelBuilder.Entity<Item>().HasData(
                new("Laptops", "Laptop #1", 900, 10, "") { Id = items[0] },
                new("Microphones", "Microphone #2", 20, 10, "") { Id = items[1] },
                new("Laptops", "Laptop #2", 650, 10, "") { Id = items[2] },
                new("Laptops", "Laptop #3", 800, 10, "") { Id = items[3] },
                new("Laptops", "Laptop #4", 500, 10, "") { Id = items[4] },
                new("Laptops", "Laptop #5", 660, 10, "") { Id = items[5] },
                new("Laptops", "Laptop #6", 500, 10, "") { Id = items[6] },
                new("Laptops", "Laptop #7", 450, 10, "") { Id = items[7] },
                new("Headsets", "Headset #1", 100, 10, "") { Id = items[8] },
                new("Headsets", "Headset #2", 300, 10, "") { Id = items[9] },
                new("Headsets", "Headset #3", 50, 10, "") { Id = items[10] },
                new("Microphones", "Microphone #1", 50, 10, "") { Id = items[11] });

            modelBuilder.Entity<Image>().HasData(
                new("https://placehold.co/150x150", items[0]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[1]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[1]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[1]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[2]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[3]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[4]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[5]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[6]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[7]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[8]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[8]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[9]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[10]) { Id = Guid.NewGuid() },
                new("https://placehold.co/150x150", items[11]) { Id = Guid.NewGuid() });

            modelBuilder.Entity<ItemConfiguration>().HasData(
                new(items[0], configs[4]),
                new(items[0], configs[7]),
                new(items[0], configs[9]),
                new(items[0], configs[15]),
                new(items[0], configs[18]),
                new(items[1], configs[2]),
                new(items[1], configs[6]),
                new(items[1], configs[10]),
                new(items[1], configs[13]),
                new(items[1], configs[20]),
                new(items[2], configs[0]),
                new(items[2], configs[5]),
                new(items[3], configs[2]),
                new(items[3], configs[7]),
                new(items[4], configs[4]),
                new(items[4], configs[8]),
                new(items[5], configs[9]),
                new(items[5], configs[14]),
                new(items[6], configs[10]),
                new(items[6], configs[14]),
                new(items[6], configs[21]));

            modelBuilder.Entity<AdressDetails>().HasData(
                new(userId1, "Śląsk", "Bielsko-Biała", "43-300", "3 Maja", "17", "91") { Id = Guid.NewGuid() },
                new(userId2, "Dolny Śląsk", "Wrocław", "50-383", "Fryderyka Joliot-Curie", "15") { Id = Guid.NewGuid() });

            modelBuilder.Entity<CustomerDetails>().HasData(
                new(userId1, "Bartłomiej", "Żurowski", "29 02 2024 0", "bartżur@tlen.o2") { Id = Guid.NewGuid() },
                new(userId2, "Stanisław", "August", "03 05 1791 0", "stan3@rp.on") { Id = Guid.NewGuid() });

            modelBuilder.Entity<OrderedProduct>().HasData(
                new(userId1, supplierId1, "aaa", 2, 50) { Id = Guid.NewGuid() },
                new(userId1, supplierId1, "aab", 1, 250) { Id = Guid.NewGuid() },
                new(userId2, supplierId1, "aac", 1, 500) { Id = Guid.NewGuid() },
                new(userId2, supplierId2, "aba", 3, 150) { Id = Guid.NewGuid() },
                new(userId1, supplierId2, "aba", 1, 150) { Id = Guid.NewGuid() },
                new(userId2, supplierId2, "abb", 4, 50) { Id = Guid.NewGuid() },
                new(userId1, supplierId1, "baa", 1, 500) { Id = Guid.NewGuid() },
                new(userId2, supplierId1, "bab", 1, 350) { Id = Guid.NewGuid() },
                new(userId2, supplierId1, "aab", 2, 150) { Id = Guid.NewGuid() });
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<OrderedProduct> OrderedProducts { get; set; }

        public DbSet<AdressDetails> AdressDetails { get; set; }

        public DbSet<CustomerDetails> CustomerDetails { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
