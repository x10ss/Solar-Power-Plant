using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantApi.Data;
using PlantApi.Model;

namespace PlantApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SolarPowerPlantController : Controller
    {
        ILogger<SolarPowerPlantController> Logger;
        private readonly MyDbContext _context;

        public SolarPowerPlantController(MyDbContext context, ILogger<SolarPowerPlantController> logger)
        {
            _context = context;
            Logger = logger;
        }

        // CREATE A SOLAR POWER PLANT
        [Authorize]
        [HttpPost]
        [ActionName("AddPlant")]
        [ProducesResponseType(typeof(SolarPowerPlant), 201)]
        public async Task<ActionResult<SolarPowerPlant>> Post([FromBody] CreatePlantModel plant)
        {
            SolarPowerPlant newPlant = new SolarPowerPlant
            {
                DateInstalled = plant.DateInstalled,
                PlantName = plant.PlantName,
                PlantInstalledPower = plant.PlantInstalledPower,
                Latitude = plant.Latitude,
                Longitude = plant.Longitude,
            };
            _context.SolarPowerPlants.Add(newPlant);
            await _context.SaveChangesAsync();
            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "New plant created: " + newPlant.ToString());
            return CreatedAtAction(nameof(GetPlant), new { id = newPlant.Id }, newPlant);

        }

        // GET SOLAR POWER PLANT BY ID
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SolarPowerPlant), 200)]
        public async Task<ActionResult<SolarPowerPlant>> GetPlant(int id)
        {
            var plant = await _context.SolarPowerPlants.FindAsync(id);

            if (plant == null)
            {
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                    "Plant with id: " + id.ToString() + " doesn't exist.");
                return NotFound();
            }
            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "Plant info sent: " + plant.ToString());
            return Ok(plant);
        }

        // UPDATE SOLAR POWER PLANT BY ID
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SolarPowerPlant), 200)]
        public async Task<ActionResult<SolarPowerPlant>> UpdatePlant(int id, CreatePlantModel plant)
        {
            var plantToUpdate = await _context.SolarPowerPlants.FindAsync(id);
            if (plantToUpdate == null)
            {
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                    "Plant with id: " + id.ToString() + " doesn't exist.");
                return NotFound();
            }
            plantToUpdate.PlantName = plant.PlantName;
            plantToUpdate.Latitude = plant.Latitude;
            plantToUpdate.Longitude = plant.Longitude;
            plantToUpdate.PlantInstalledPower = plantToUpdate.PlantInstalledPower;
            plantToUpdate.DateInstalled = plantToUpdate.DateInstalled;

            await _context.SaveChangesAsync();
            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "Plant info with id: " + id.ToString() + " sent: " + plant.ToString());
            return CreatedAtAction(nameof(GetPlant), new { id = plantToUpdate.Id }, plantToUpdate);
        }

        // DELETE SOLAR POWER PLANT BY ID
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.SolarPowerPlants.FindAsync(id);
            if (plant == null)
            {
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                    "Plant with id: " + id.ToString() + " doesn't exist.");
                return NotFound();
            }
            _context.SolarPowerPlants.Remove(plant);
            await _context.SaveChangesAsync();
            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "Plant with id: " + id.ToString() + " deleted.");
            return NoContent();
        }
    }
}
