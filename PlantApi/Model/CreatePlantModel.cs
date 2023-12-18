namespace PlantApi.Model
{
    public class CreatePlantModel
    {
        public string PlantName { get; set; }
        public float PlantInstalledPower { get; set; }
        public DateTime DateInstalled { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
