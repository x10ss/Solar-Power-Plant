using PlantApi.Model;

namespace PlantApi.Data
{
    public class DBInit
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MyDbContext>();
                context.Database.EnsureCreated();

                /// MISC
                DateTime Today = DateTime.UtcNow;
                Random r = new Random();

                ////// CONFIGURE NUMBER OF SOLAR POWER PLANTS THAT ARE GOING TO BE CREATED
                int SolarPlantCount = r.Next(13, 36);
                //SolarPlantCount = 10;

                ////// CONFIGURE DATA ITEMS FOR EACH SOLAR POWER PLANT
                int HistoryLenght = r.Next(99, 360);
                //HistoryLenght = 200;

                //// CREATE HISTORICAL SOLAR POWER PLANTS
                for (int i = 0; i < SolarPlantCount; i++)
                {
                    SolarPowerPlant spp = new SolarPowerPlant
                    {
                        DateInstalled = DateTime.Now.AddMonths(r.Next(-36, 0)),
                        Latitude = r.Next(-90, 90),
                        Longitude = r.Next(-180, 180),
                        PlantName = "plantname" + i.ToString(),
                        PlantInstalledPower = r.Next(1000, 10000),
                    };
                    context.SolarPowerPlants.Add(spp);
                    context.SaveChanges();
                    context.Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                        "Solar Power Plant " + spp.PlantName + "was created with a id: " + spp.Id);
                    ////////// CREATE HISTORICAL DATA FOR THIS SOLAR POWER PLANT
                    for (int j = 0; j < HistoryLenght; j++)
                    {
                        SolarPowerPlantData sppd = new SolarPowerPlantData
                        {
                            SolarPowerPlant = spp,
                            SolarPowerPlantId = spp.Id,
                            ActualPower = (float)r.NextDouble() * spp.PlantInstalledPower,
                            ForecastedPower = (float)r.NextDouble() * spp.PlantInstalledPower,
                            GranulomCount = -j,
                        };
                        context.SolarPowerPlantsData.Add(sppd);
                        context.SaveChanges();
                        context.Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                            "Solar Power Plant " + spp.Id + " " + spp.PlantName + " Data with a id: " + sppd.Id + " with a timeline: " + sppd.GranulomCount + " position was created.");
                    }
                }
            }
        }
    }
}
