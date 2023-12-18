namespace PlantApi.Model
{
    public class GetProductionModel
    {
        public bool IsForcasted { get; set; }
        public bool IsGranularityHourly { get; set; }
        public int TimeSpanStart { get; set; }
        public int FutureLenght { get; set; }
        public int PlantId { get; set; }
    }
}
