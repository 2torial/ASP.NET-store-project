﻿using ASP.NET_store_project.Server.Data.DataOutsorced;
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

            modelBuilder.Entity<Supplier>().HasData(
                new("SupplierA", new("https://localhost:5173/")) { Id = supplierId1 },
                new("SupplierB", new("https://localhost:5173/"), 200) { Id = supplierId2 });

            modelBuilder.Entity<Category>().HasData(
                new("Laptops", "Laptops/Notebooks/Ultrabooks"),
                new("Headsets", "Headsets"),
                new("Microphones", "Microphones"));

            //modelBuilder.Entity<Configuration>().HasData(
            //    new(1, "RAM Memory", "4 GB", 4),
            //    new(2, "RAM Memory", "8 GB", 8),
            //    new(3, "RAM Memory", "16 GB", 16),
            //    new(4, "RAM Memory", "32 GB", 32),
            //    new(5, "RAM Memory", "64 GB", 64),
            //    new(6, "RAM Memory", "No Memory"),
            //    new(7, "System", "Windows 10", 1),
            //    new(8, "System", "Windows 11", 2),
            //    new(9, "System", "MacOS", 11),
            //    new(10, "System", "No System"),
            //    new(11, "Disk", "SSD", 1),
            //    new(12, "Disk", "SSD", 2),
            //    new(13, "Disk", "No Disk"),
            //    new(14, "Disk Capacity", "512 GB", 512),
            //    new(15, "Disk Capacity", "1024 GB", 1024),
            //    new(16, "Disk Capacity", "2048 GB", 2048),
            //    new(17, "Disk Capacity", "4096 GB", 4096),
            //    new(18, "Processor", "Intel Core i3", 1),
            //    new(19, "Processor", "Intel Core i5", 1),
            //    new(20, "Processor", "Intel Core i7", 1),
            //    new(21, "Processor", "Intel Core i9", 1),
            //    new(22, "Processor", "Ryzen 3", 2),
            //    new(23, "Processor", "Ryzen 5", 2),
            //    new(24, "Processor", "Ryzen 7", 2),
            //    new(25, "Processor", "Ryzen 9", 2),
            //    new(26, "Processor", "No Processor"),
            //    new(27, "Cord Length", "1 m", 1),
            //    new(28, "Cord Length", "2 m", 2));

            //modelBuilder.Entity<Item>().HasData(
            //    new(1, "Laptops", "Laptop #1", 900),
            //    new(2, "Laptops", "Laptop #2", 650),
            //    new(3, "Laptops", "Laptop #3", 800),
            //    new(4, "Laptops", "Laptop #4", 500),
            //    new(5, "Laptops", "Laptop #5", 660),
            //    new(6, "Laptops", "Laptop #6", 500),
            //    new(7, "Laptops", "Laptop #7", 450),
            //    new(8, "Headsets", "Headset #1", 100),
            //    new(9, "Headsets", "Headset #2", 300),
            //    new(10, "Headsets", "Headset #3", 50),
            //    new(11, "Microphones", "Microphone #1", 50),
            //    new(12, "Microphones", "Microphone #2", 20));

            //modelBuilder.Entity<Image>().HasData(
            //    new(1, "https://placehold.co/150x150", 1),
            //    new(2, "https://placehold.co/150x150", 2),
            //    new(3, "https://placehold.co/150x150", 2),
            //    new(4, "https://placehold.co/150x150", 2),
            //    new(5, "https://placehold.co/150x150", 3),
            //    new(6, "https://placehold.co/150x150", 4),
            //    new(7, "https://placehold.co/150x150", 5),
            //    new(8, "https://placehold.co/150x150", 6),
            //    new(9, "https://placehold.co/150x150", 7),
            //    new(10, "https://placehold.co/150x150", 8),
            //    new(11, "https://placehold.co/150x150", 9),
            //    new(12, "https://placehold.co/150x150", 9),
            //    new(13, "https://placehold.co/150x150", 10),
            //    new(14, "https://placehold.co/150x150", 11),
            //    new(15, "https://placehold.co/150x150", 12));

            //modelBuilder.Entity<ItemConfiguration>().HasData(
            //    new(1, 5),
            //    new(1, 8),
            //    new(1, 10),
            //    new(1, 16),
            //    new(1, 19),
            //    new(2, 3),
            //    new(2, 7),
            //    new(2, 11),
            //    new(2, 14),
            //    new(2, 21),
            //    new(3, 1),
            //    new(3, 6),
            //    new(4, 3),
            //    new(4, 8),
            //    new(5, 5),
            //    new(5, 9),
            //    new(6, 10),
            //    new(6, 15),
            //    new(7, 11),
            //    new(7, 15),
            //    new(7, 22));

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
    }
}
