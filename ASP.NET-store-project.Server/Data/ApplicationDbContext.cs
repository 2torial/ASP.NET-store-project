using Microsoft.EntityFrameworkCore;

namespace ASP.NET_store_project.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("StoreDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Item> Items { get; set; }
    }
}
