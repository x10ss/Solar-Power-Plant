using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantApi.Data;
using PlantApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace PlantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext _context;
        ILogger<LoginController> Logger;
        private IConfiguration _config;

        public LoginController(MyDbContext context, IConfiguration config, ILogger<LoginController> logger)
        {
            _context = context;
            _config = config;
            Logger = logger;
        }

        // LOGIN TO GET A JWT KEY
        [HttpPost]
        public IActionResult Post([FromBody] LoginModel loginRequest)
        {
            int user = _context.SolarPowerPlantUsers.Where<SolarPowerPlantUser>(x => x.Name == loginRequest.Name && x.Password == loginRequest.Password).Count();
            if (user == 0)
            {
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true),
                    "User: " + loginRequest.Name + " not found, or wrong password.");
                return NotFound();
            }
            else
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  null,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                Logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), "Information", true), loginRequest.ToString() +
                    " user loged in and recieved a JWT token: " + token.ToString());
                return Ok(token);
            }
        }
    }
}
