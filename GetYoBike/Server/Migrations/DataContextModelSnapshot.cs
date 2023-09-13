﻿// <auto-generated />
using System;
using GetYoBike.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GetYoBike.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("GetYoBike.Server.Entities.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.BikeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("BikeTypes");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.Rent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CardCVC")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.Property<string>("CardExpMonth")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CardExpYear")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CardNr")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditPIN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDiscounted")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("RentedBikeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RenterUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RentedBikeId");

                    b.HasIndex("RenterUserId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.Bike", b =>
                {
                    b.HasOne("GetYoBike.Server.Entities.BikeType", "Type")
                        .WithMany("Bikes")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.Rent", b =>
                {
                    b.HasOne("GetYoBike.Server.Entities.Bike", "RentedBike")
                        .WithMany("Rents")
                        .HasForeignKey("RentedBikeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GetYoBike.Server.Entities.User", "RenterUser")
                        .WithMany("Rents")
                        .HasForeignKey("RenterUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RentedBike");

                    b.Navigation("RenterUser");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.Bike", b =>
                {
                    b.Navigation("Rents");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.BikeType", b =>
                {
                    b.Navigation("Bikes");
                });

            modelBuilder.Entity("GetYoBike.Server.Entities.User", b =>
                {
                    b.Navigation("Rents");
                });
#pragma warning restore 612, 618
        }
    }
}
