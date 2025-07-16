using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ReWoWets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ReWowDbContext _context;
        public AuthController(ReWowDbContext context)
        {
            _context = context;
        }

        public class LoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == login.Username && a.Password == login.Password);
            if (admin == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("YourSuperSecretKey123456789123456789!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim("adminId", admin.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString, message = "Login exitoso", adminId = admin.Id });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto register)
        {
            if (string.IsNullOrWhiteSpace(register.Username) || string.IsNullOrWhiteSpace(register.Password))
                return BadRequest(new { message = "Username and password are required." });

            var exists = await _context.Admins.AnyAsync(a => a.Username == register.Username);
            if (exists)
                return BadRequest(new { message = "Username already exists." });

            var admin = new Models.Admin
            {
                Username = register.Username,
                Password = register.Password
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Admin registered successfully." });
        }

    }
} 