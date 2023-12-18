﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlantApi.Data;

#nullable disable

namespace PlantApi.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PlantApi.Model.SolarPowerPlant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateInstalled")
                        .HasColumnType("datetime2");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<float>("PlantInstalledPower")
                        .HasColumnType("real");

                    b.Property<string>("PlantName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SolarPowerPlant", (string)null);
                });

            modelBuilder.Entity("PlantApi.Model.SolarPowerPlantData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("ActualPower")
                        .HasColumnType("real");

                    b.Property<float>("ForecastedPower")
                        .HasColumnType("real");

                    b.Property<int>("GranulomCount")
                        .HasColumnType("int");

                    b.Property<int>("SolarPowerPlantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SolarPowerPlantId");

                    b.HasIndex("GranulomCount", "SolarPowerPlantId")
                        .IsUnique();

                    b.ToTable("SolarPowerPlantData", (string)null);
                });

            modelBuilder.Entity("PlantApi.Model.SolarPowerPlantUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SolarPowerPlantUser", (string)null);
                });

            modelBuilder.Entity("PlantApi.Model.SolarPowerPlantData", b =>
                {
                    b.HasOne("PlantApi.Model.SolarPowerPlant", "SolarPowerPlant")
                        .WithMany("SolarPowerPlantDatas")
                        .HasForeignKey("SolarPowerPlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SolarPowerPlant");
                });

            modelBuilder.Entity("PlantApi.Model.SolarPowerPlant", b =>
                {
                    b.Navigation("SolarPowerPlantDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
