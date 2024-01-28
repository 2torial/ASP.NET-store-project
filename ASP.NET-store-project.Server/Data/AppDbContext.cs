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
            modelBuilder.Entity<SelectedItem>()
                .HasOne(e => e.Item)
                .WithOne();

            modelBuilder.Entity<Item>()
                .HasOne(e => e.Category)
                .WithOne();

            modelBuilder.Entity<Item>()
                .HasMany(e => e.Configurations)
                .WithMany()
                .UsingEntity(join => join.ToTable("ItemConfiguration"));

            modelBuilder.Entity<OrderStatus>()
                .HasOne(e => e.Order)
                .WithMany(e => e.StatusHistory)
                .HasForeignKey(e => e.OrderId);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Item> Items { get; set; }
    }
}
