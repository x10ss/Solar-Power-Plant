using Microsoft.AspNetCore.Mvc;
using PlantApi.Data;
using PlantApi.Model;


namespace PlantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : Controller
    {
        ILogger<WeatherForecastController> WeatherLogger;
        ILogger<ProductionController> Logger;
        private readonly MyDbContext _context;

        public ProductionController(MyDbContext context, ILogger<ProductionController> logger, ILogger<WeatherForecastController> weatherLogger)
        {
            _context = context;
            Logger = logger;
            WeatherLogger = weatherLogger;
        }

        // GET PRODUCTION DATA
        [HttpPost]
        [ActionName("GetProductionData")]
        [ProducesResponseType(typeof(List<SolarPowerPlantData>), 201)]
        public async Task<ActionResult<List<SolarPowerPlantDataModel>>> Post([FromBody] GetProductionModel getProductionModel)
        {
            ////go through the timespan
            for (int i = 0; i < getProductionModel.FutureLenght; i++)
            {
                ////check if there is a missing granulom on the timeline i.e. there is no value in the database for a chosen timespan
                if (_context.SolarPowerPlantsData.Any<SolarPowerPlantData>(x => x.GranulomCount != getProductionModel.TimeSpanStart + i))
                {
                    List<SolarPowerPlantDataModel> sppdmList = new List<SolarPowerPlantDataModel>();
                    SolarPowerPlant solarPowerPlant = _context.SolarPowerPlants.Where(x => x.Id == getProductionModel.PlantId).ToList<SolarPowerPlant>().FirstOrDefault();
                    try
                    {
                        for (int j = getProductionModel.TimeSpanStart; j < getProductionModel.TimeSpanStart + getProductionModel.FutureLenght; j++)
                        {
                            WeatherForecastController forecastWeatherController = new WeatherForecastController(WeatherLogger);
                            var weather = forecastWeatherController.Get();
                            float percentage = ((float)(weather.TemperatureF - weather.TemperatureC)) / 100;
                            //// create granulom timespot i.e. FORECAST / SET ACTUAL
                            SolarPowerPlantData sppd = new SolarPowerPlantData
                            {
                                SolarPowerPlant = solarPowerPlant,
                                SolarPowerPlantId = solarPowerPlant.Id,
                                ActualPower = (float)(Random.Shared.NextDouble() * solarPowerPlant.PlantInstalledPower),
                                ForecastedPower = solarPowerPlant.PlantInstalledPower * percentage,
                                GranulomCount = j
                            };
                            //// check if there is a granulom already in the database, if no than create a new one
                            if (!(_context.SolarPowerPlantsData.Any(x => x.GranulomCount == sppd.GranulomCount && x.SolarPowerPlantId == sppd.SolarPowerPlantId)))
                            {
                                _context.SolarPowerPlantsData.Add(sppd);
                                _context.SaveChanges();

                                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                                    "Adding new granulom to the db. Granulom: " + sppd.GranulomCount.ToString());
                                SolarPowerPlantDataModel sppdm = new SolarPowerPlantDataModel
                                {
                                    SolarPowerPlantId = solarPowerPlant.Id,
                                    Power = getProductionModel.IsForcasted ? sppd.ForecastedPower : sppd.ActualPower,
                                    GranulomCount = sppd.GranulomCount
                                };
                                sppdmList.Add(sppdm);
                                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                                    "Adding new granulom to the response list. Granulom: " + sppdm.GranulomCount.ToString());
                            }
                            /// if yes than add that database granulom to the list that will be returned
                            else
                            {
                                SolarPowerPlantData existingGranulom = _context.SolarPowerPlantsData.Where(x => x.GranulomCount == sppd.GranulomCount && x.SolarPowerPlantId == solarPowerPlant.Id).FirstOrDefault();
                                SolarPowerPlantDataModel existingGranulomSppdm = new SolarPowerPlantDataModel
                                {
                                    SolarPowerPlantId = solarPowerPlant.Id,
                                    Power = getProductionModel.IsForcasted ? existingGranulom.ForecastedPower : existingGranulom.ActualPower,
                                    GranulomCount = existingGranulom.GranulomCount
                                };
                                sppdmList.Add(existingGranulomSppdm);
                                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                                    "Adding existing granulom to the response list. Granulom: " + existingGranulomSppdm.GranulomCount.ToString());
                            }
                        }
                    }
                    //////// probably no plant with such id
                    catch (Exception e)
                    {
                        Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                            "Solar Power Plant does not exist. Exception was thrown: " + e.Message);
                        return NotFound();
                    }
                    ///////// IsGranularityHourly bool false means 15 minutes
                    if (!getProductionModel.IsGranularityHourly)
                    {
                        Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                            "Production data sent: " + sppdmList.ToString() + " with a granularity of 15 minutes");
                        return Ok(sppdmList);
                    }
                    /////////// handle hourly granularity, IsGranualityHourly true bool
                    else
                    {
                        List<SolarPowerPlantDataModel> sppdmListHourly = new List<SolarPowerPlantDataModel>();
                        try
                        {
                            List<SolarPowerPlantDataModel> takeFourSppdm = new List<SolarPowerPlantDataModel>();
                            int quotient = sppdmList.Count / 4;
                            for (int j = 0; j < (quotient * 3); j += 4)
                            {
                                takeFourSppdm = sppdmList.Skip(j).Take(4).ToList();
                                SolarPowerPlantDataModel hourlySppdm = new SolarPowerPlantDataModel
                                {
                                    GranulomCount = takeFourSppdm[0].GranulomCount,
                                    Power = (takeFourSppdm[0].Power + takeFourSppdm[1].Power + takeFourSppdm[2].Power + takeFourSppdm[3].Power) / 4,
                                    SolarPowerPlantId = takeFourSppdm[0].SolarPowerPlantId
                                };
                                sppdmListHourly.Add(hourlySppdm);
                            }
                            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                                "Production data sent: " + sppdmListHourly.ToString() + "with a granularity of an hour");
                            return Ok(sppdmListHourly);
                        }
                        catch (Exception e)
                        {
                            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                                "Production data sent: " + sppdmListHourly.ToString() + "with a granularity of an hour, but the exception " + e.Message + " was thrown.");
                        }

                    }
                }
            }
            return StatusCode(500, "An error occurred. Please try again later.");
        }
    }
}
