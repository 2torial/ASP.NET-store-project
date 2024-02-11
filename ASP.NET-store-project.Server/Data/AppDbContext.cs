using Microsoft.EntityFrameworkCore;

namespace ASP.NET_store_project.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(b => b.IsAdmin)
                .HasDefaultValue(false);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.AdressDetails)
                .WithOne()
                .HasForeignKey<Order>(e => e.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.CustomerDetails)
                .WithOne()
                .HasForeignKey<Order>(e => e.OrderId);

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Item>()
                .ToTable("Item");

            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<Order>()
                .ToTable("Order");

            modelBuilder.Entity<SortingMethod>()
                .ToTable("SortingMethod");

            modelBuilder.Entity<Item>()
                .HasMany(e => e.Configurations)
                .WithMany(e => e.Items)
                .UsingEntity<ItemConfiguration>();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.StatusHistory)
                .WithMany()
                .UsingEntity<OrderStatus>(
                    e => {
                        e.Property(e => e.DateOfChange).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    });

            modelBuilder.Entity<User>().HasData(
                new("user", "user"),
                new("root", "root", true));

            modelBuilder.Entity<Category>().HasData(
                new("Laptops", "Laptops/Notebooks/Ultrabooks"),
                new("Headsets", "Headsets"),
                new("Microphones", "Microphones"));

            modelBuilder.Entity<Configuration>().HasData(
                new(1, "RAM Memory", "4 GB", 4),
                new(2, "RAM Memory", "8 GB", 8),
                new(3, "RAM Memory", "16 GB", 16),
                new(4, "RAM Memory", "32 GB", 32),
                new(5, "RAM Memory", "64 GB", 64),
                new(6, "RAM Memory", "No Memory"),
                new(7, "System", "Windows 10", 1),
                new(8, "System", "Windows 11", 2),
                new(9, "System", "MacOS", 11),
                new(10, "System", "No System"),
                new(11, "Disk", "SSD", 1),
                new(12, "Disk", "SSD", 2),
                new(13, "Disk", "No Disk"),
                new(14, "Disk Capacity", "512 GB", 512),
                new(15, "Disk Capacity", "1024 GB", 1024),
                new(16, "Disk Capacity", "2048 GB", 2048),
                new(17, "Disk Capacity", "4096 GB", 4096),
                new(18, "Processor", "Intel Core i3", 1),
                new(19, "Processor", "Intel Core i5", 1),
                new(20, "Processor", "Intel Core i7", 1),
                new(21, "Processor", "Intel Core i9", 1),
                new(22, "Processor", "Ryzen 3", 2),
                new(23, "Processor", "Ryzen 5", 2),
                new(24, "Processor", "Ryzen 7", 2),
                new(25, "Processor", "Ryzen 9", 2),
                new(26, "Processor", "No Processor"),
                new(27, "Cord Length", "1 m", 1),
                new(28, "Cord Length", "2 m", 2));

            modelBuilder.Entity<Item>().HasData(
                new(1, "Laptops", "Laptop #1", 900),
                new(2, "Laptops", "Laptop #2", 650),
                new(3, "Laptops", "Laptop #3", 800),
                new(4, "Laptops", "Laptop #4", 500),
                new(5, "Laptops", "Laptop #5", 660),
                new(6, "Laptops", "Laptop #6", 500),
                new(7, "Laptops", "Laptop #7", 450),
                new(8, "Headsets", "Headset #1", 100),
                new(9, "Headsets", "Headset #2", 300),
                new(10, "Headsets", "Headset #3", 50),
                new(11, "Microphones", "Microphone #1", 50),
                new(12, "Microphones", "Microphone #2", 20));

            modelBuilder.Entity<Image>().HasData(
                new(1, "https://placehold.co/150x150", 1),
                new(2, "https://placehold.co/150x150", 2),
                new(3, "https://placehold.co/150x150", 2),
                new(4, "https://placehold.co/150x150", 2),
                new(5, "https://placehold.co/150x150", 3),
                new(6, "https://placehold.co/150x150", 4),
                new(7, "https://placehold.co/150x150", 5),
                new(8, "https://placehold.co/150x150", 6),
                new(9, "https://placehold.co/150x150", 7),
                new(10, "https://placehold.co/150x150", 8),
                new(11, "https://placehold.co/150x150", 9),
                new(12, "https://placehold.co/150x150", 9),
                new(13, "https://placehold.co/150x150", 10),
                new(14, "https://placehold.co/150x150", 11),
                new(15, "https://placehold.co/150x150", 12));

            modelBuilder.Entity<ItemConfiguration>().HasData(
                new(1, 5),
                new(1, 8),
                new(1, 10),
                new(1, 16),
                new(1, 19),
                new(2, 3),
                new(2, 7),
                new(2, 11),
                new(2, 14),
                new(2, 21),
                new(3, 1),
                new(3, 6),
                new(4, 3),
                new(4, 8),
                new(5, 5),
                new(5, 9),
                new(6, 10),
                new(6, 15),
                new(7, 11),
                new(7, 15),
                new(7, 22));

            modelBuilder.Entity<Status>().HasData(
                new("Pending"),
                new("Preparing"),
                new("Awaiting Delivery"),
                new("Sent"),
                new("Delivered"),
                new("Returned"),
                new("Canceled"));

            modelBuilder.Entity<AdressDetails>().HasData(
                new(1, "Śląsk", "Bielsko-Biała", "43-300", "3 Maja", "17", "91"),
                new(2, "Dolny Śląsk", "Wrocław", "50-383", "Fryderyka Joliot-Curie", "15"));

            modelBuilder.Entity<CustomerDetails>().HasData(
                new(1, "Bartłomiej", "Żurowski", "29 02 2024 0", "bartżur@tlen.o2"),
                new(2, "Stanisław", "August", "03 05 1791 0", "stan3@rp.on"));

            modelBuilder.Entity<Order>().HasData(
                new(1, "user"),
                new(2, "root"));

            modelBuilder.Entity<SelectedItem>().HasData(
                new(1, 1, "user", 1, 1),
                new(2, 8, "user", 1, 1),
                new(3, 12, "user", 1, 1),
                new(4, 4, "root", 10, 2),
                new(5, 1, "root", 1, 2),
                new(6, 2, "user", 1),
                new(7, 9, "user", 2),
                new(8, 3, "root", 4),
                new(9, 9, "root", 3));

            modelBuilder.Entity<SortingMethod>().HasData(
                new("Price: Lowest to Highest", "Price", true),
                new("Price: Highest to Lowest", "Price", false),
                new("Name: Ascending", "Name", true),
                new("Name: Descending", "Name", false));
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SortingMethod> SortingMethods { get; set; }
    }
}
