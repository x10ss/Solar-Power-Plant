namespace PlantApi.Model
{
    /// <summary>
    /// ENTITY MODEL USED TO CREATE THE DATABASE
    /// </summary>
    public class SolarPowerPlantUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class SolarPowerPlant
    {
        public int Id { get; set; }
        public string? PlantName { get; set; }
        public float PlantInstalledPower { get; set; }
        public DateTime DateInstalled { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public virtual ICollection<SolarPowerPlantData> SolarPowerPlantDatas { get; set; } = new List<SolarPowerPlantData>();
    }
    public class SolarPowerPlantData
    {
        public int Id { get; set; }
        public int GranulomCount { get; set; }
        public float ActualPower { get; set; }
        public float ForecastedPower { get; set; }
        public int SolarPowerPlantId { get; set; }
        public virtual SolarPowerPlant SolarPowerPlant { get; set; } = new();
    }
}
