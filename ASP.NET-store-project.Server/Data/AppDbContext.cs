using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace ASP.NET_store_project.Server.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("StoreDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(b => b.IsAdmin)
                .HasDefaultValue(false);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.AdressDetails)
                .WithOne()
                .HasForeignKey<Order>(e => e.Id);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.CustomerDetails)
                .WithOne()
                .HasForeignKey<Order>(e => e.Id);

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Item>()
                .ToTable("Item");

            modelBuilder.Entity<Customer>()
                .ToTable("Customer");

            modelBuilder.Entity<Order>()
                .ToTable("Order");

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

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}
