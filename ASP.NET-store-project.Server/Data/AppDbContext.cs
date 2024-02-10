using Microsoft.EntityFrameworkCore;

namespace ASP.NET_store_project.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
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

            modelBuilder.Entity<Customer>()
                .ToTable("Customer");

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

            modelBuilder.Entity<Customer>().HasData(
                new Customer("user", "user"),
                new Customer("root", "root", true));

            modelBuilder.Entity<Category>().HasData(
                new Category("Laptops", "Laptops/Notebooks/Ultrabooks"),
                new Category("Headsets", "Headsets"),
                new Category("Microphones", "Microphones"));

            modelBuilder.Entity<Configuration>().HasData(
                new Configuration(1, "RAM Memory", "4 GB", 4),
                new Configuration(2, "RAM Memory", "8 GB", 8),
                new Configuration(3, "RAM Memory", "16 GB", 16),
                new Configuration(4, "RAM Memory", "32 GB", 32),
                new Configuration(5, "RAM Memory", "64 GB", 64),
                new Configuration(6, "RAM Memory", "No Memory"),
                new Configuration(7, "System", "Windows 10", 1),
                new Configuration(8, "System", "Windows 11", 2),
                new Configuration(9, "System", "MacOS", 11),
                new Configuration(10, "System", "No System"),
                new Configuration(11, "Disk", "SSD", 1),
                new Configuration(12, "Disk", "SSD", 2),
                new Configuration(13, "Disk", "No Disk"),
                new Configuration(14, "Disk Capacity", "512 GB", 512),
                new Configuration(15, "Disk Capacity", "1024 GB", 1024),
                new Configuration(16, "Disk Capacity", "2048 GB", 2048),
                new Configuration(17, "Disk Capacity", "4096 GB", 4096),
                new Configuration(18, "Processor", "Intel Core i3", 1),
                new Configuration(19, "Processor", "Intel Core i5", 1),
                new Configuration(20, "Processor", "Intel Core i7", 1),
                new Configuration(21, "Processor", "Intel Core i9", 1),
                new Configuration(22, "Processor", "Ryzen 3", 2),
                new Configuration(23, "Processor", "Ryzen 5", 2),
                new Configuration(24, "Processor", "Ryzen 7", 2),
                new Configuration(25, "Processor", "Ryzen 9", 2),
                new Configuration(26, "Processor", "No Processor"),
                new Configuration(27, "Cord Length", "1 m", 1),
                new Configuration(28, "Cord Length", "2 m", 2));

            modelBuilder.Entity<Item>().HasData(
                new Item(1, "Laptops", "Laptop #1", 900),
                new Item(2, "Laptops", "Laptop #2", 650),
                new Item(3, "Laptops", "Laptop #3", 800),
                new Item(4, "Laptops", "Laptop #4", 500),
                new Item(5, "Laptops", "Laptop #5", 660),
                new Item(6, "Laptops", "Laptop #6", 500),
                new Item(7, "Laptops", "Laptop #7", 450),
                new Item(8, "Headsets", "Headset #1", 100),
                new Item(9, "Headsets", "Headset #2", 300),
                new Item(10, "Headsets", "Headset #3", 50),
                new Item(11, "Microphones", "Microphone #1", 50),
                new Item(12, "Microphones", "Microphone #2", 20));

            modelBuilder.Entity<Image>().HasData(
                new Image(1, "https://placehold.co/150x150", 1),
                new Image(2, "https://placehold.co/150x150", 2),
                new Image(3, "https://placehold.co/150x150", 2),
                new Image(4, "https://placehold.co/150x150", 2),
                new Image(5, "https://placehold.co/150x150", 3),
                new Image(6, "https://placehold.co/150x150", 4),
                new Image(7, "https://placehold.co/150x150", 5),
                new Image(8, "https://placehold.co/150x150", 6),
                new Image(9, "https://placehold.co/150x150", 7),
                new Image(10, "https://placehold.co/150x150", 8),
                new Image(11, "https://placehold.co/150x150", 9),
                new Image(12, "https://placehold.co/150x150", 9),
                new Image(13, "https://placehold.co/150x150", 10),
                new Image(14, "https://placehold.co/150x150", 11),
                new Image(15, "https://placehold.co/150x150", 12));

            modelBuilder.Entity<ItemConfiguration>().HasData(
                new ItemConfiguration(1, 5),
                new ItemConfiguration(1, 8),
                new ItemConfiguration(1, 10),
                new ItemConfiguration(1, 16),
                new ItemConfiguration(1, 19),
                new ItemConfiguration(2, 3),
                new ItemConfiguration(2, 7),
                new ItemConfiguration(2, 11),
                new ItemConfiguration(2, 14),
                new ItemConfiguration(2, 21),
                new ItemConfiguration(3, 1),
                new ItemConfiguration(3, 6),
                new ItemConfiguration(4, 3),
                new ItemConfiguration(4, 8),
                new ItemConfiguration(5, 5),
                new ItemConfiguration(5, 9),
                new ItemConfiguration(6, 10),
                new ItemConfiguration(6, 15),
                new ItemConfiguration(7, 11),
                new ItemConfiguration(7, 15),
                new ItemConfiguration(7, 22));

            modelBuilder.Entity<Status>().HasData(
                new Status("Pending"),
                new Status("Preparing"),
                new Status("Awaiting Delivery"),
                new Status("Sent"),
                new Status("Delivered"),
                new Status("Returned"),
                new Status("Canceled"));

            modelBuilder.Entity<AdressDetails>().HasData(
                new AdressDetails(1, "Śląsk", "Bielsko-Biała", "43-300", "3 Maja", "17", "91"),
                new AdressDetails(2, "Dolny Śląsk", "Wrocław", "50-383", "Fryderyka Joliot-Curie", "15"));

            modelBuilder.Entity<CustomerDetails>().HasData(
                new CustomerDetails(1, "Bartłomiej", "Żurowski", "29 02 2024 0", "bartżur@tlen.o2"),
                new CustomerDetails(2, "Stanisław", "August", "03 05 1791 0", "stan3@rp.on"));

            modelBuilder.Entity<Order>().HasData(
                new Order(1, "user"),
                new Order(2, "root"));

            modelBuilder.Entity<SelectedItem>().HasData(
                new SelectedItem(1, 1, "user", 1, 1),
                new SelectedItem(2, 8, "user", 1, 1),
                new SelectedItem(3, 12, "user", 1, 1),
                new SelectedItem(4, 4, "root", 10, 2),
                new SelectedItem(5, 1, "root", 1, 2),
                new SelectedItem(6, 2, "user", 1),
                new SelectedItem(7, 9, "user", 2),
                new SelectedItem(8, 3, "root", 4),
                new SelectedItem(9, 9, "root", 3));

            modelBuilder.Entity<SortingMethod>().HasData(
                new SortingMethod("Price: Lowest to Highest", "Price", true),
                new SortingMethod("Price: Highest to Lowest", "Price", false),
                new SortingMethod("Name: Ascending", "Name", true),
                new SortingMethod("Name: Descending", "Name", false));
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SortingMethod> SortingMethods { get; set; }
    }
}
