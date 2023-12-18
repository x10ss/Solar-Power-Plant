using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantApi.Data;
using PlantApi.Model;

namespace PlantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MyDbContext _context;
        ILogger<RegisterController> Logger;
        public RegisterController(MyDbContext context, ILogger<RegisterController> logger)
        {
            _context = context;
            Logger = logger;
        }

        // GET A USER BY ID
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SolarPowerPlantUser), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SolarPowerPlantUser>> GetUser(int id)
        {
            var user = await _context.SolarPowerPlantUsers.FindAsync(id);

            if (user == null)
            {
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                    "User by id " + id + " was not found.");
                return NotFound();
            }
            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "Get user by id was called, and returned the user id " + id + ": " + user.Name);
            return user;
        }

        // REGISTER A USER
        [HttpPost]
        [ActionName("AddUser")]
        [ProducesResponseType(typeof(SolarPowerPlantUser), 201)]
        public async Task<ActionResult<SolarPowerPlantUser>> Post([FromBody] LoginModel user)
        {
            var newUser = new SolarPowerPlantUser
            {
                Name = user.Name,
                Password = user.Password
            };
            try
            {
                _context.SolarPowerPlantUsers.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                    "User already exists. Exception was thrown: " + e.Message);
                return Problem("User already exists.");
            }
            Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                "New User Registered. Username: " + user.Name + " Password: " + user.Password);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
    }
}
