using Microsoft.EntityFrameworkCore;
using PlantApi.Model;

namespace PlantApi.Data
{
    public class MyDbContext : DbContext
    {
        public ILogger<MyDbContext> Logger;
        public DbSet<SolarPowerPlantUser> SolarPowerPlantUsers { get; set; }
        public DbSet<SolarPowerPlantData> SolarPowerPlantsData { get; set; }
        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }
        public MyDbContext(ILogger<MyDbContext> logger)
        {
            Logger = logger;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SolarPowerPlantDBConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Solar Power Plant Data table
            modelBuilder.Entity<SolarPowerPlantData>().ToTable("SolarPowerPlantData");
            modelBuilder.Entity<SolarPowerPlantData>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<SolarPowerPlantData>()
                .HasOne(b => b.SolarPowerPlant)
                .WithMany(p => p.SolarPowerPlantDatas)
                .HasForeignKey(b => b.SolarPowerPlantId);

            modelBuilder.Entity<SolarPowerPlantData>()
                .HasIndex(b => new { b.GranulomCount, b.SolarPowerPlantId })
                .IsUnique();


            // Configure the Plant table
            modelBuilder.Entity<SolarPowerPlant>().ToTable("SolarPowerPlant");
            modelBuilder.Entity<SolarPowerPlant>()
                .HasKey(p => p.Id);


            // Configure the User table
            modelBuilder.Entity<SolarPowerPlantUser>().ToTable("SolarPowerPlantUser");
            modelBuilder.Entity<SolarPowerPlantUser>()
            .HasKey(p => p.Id);

            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "Solar Power Pland Database Context Created");
        }
    }
}
