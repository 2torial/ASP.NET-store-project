﻿// <auto-generated />
using System;
using ASP.NET_store_project.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ASP.NET_store_project.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class AddDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Category", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Type");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ItemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Page")
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Specification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Configuration")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Specification");
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ItemSpecification", b =>
                {
                    b.Property<int>("ItemsId")
                        .HasColumnType("integer");

                    b.Property<int>("SpecificationsId")
                        .HasColumnType("integer");

                    b.HasKey("ItemsId", "SpecificationsId");

                    b.HasIndex("SpecificationsId");

                    b.ToTable("ItemSpecification");
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Image", b =>
                {
                    b.HasOne("ASP.NET_store_project.Server.Data.Item", null)
                        .WithMany("Images")
                        .HasForeignKey("ItemId");
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Item", b =>
                {
                    b.HasOne("ASP.NET_store_project.Server.Data.Category", "Type")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ItemSpecification", b =>
                {
                    b.HasOne("ASP.NET_store_project.Server.Data.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASP.NET_store_project.Server.Data.Specification", null)
                        .WithMany()
                        .HasForeignKey("SpecificationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ASP.NET_store_project.Server.Data.Item", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
