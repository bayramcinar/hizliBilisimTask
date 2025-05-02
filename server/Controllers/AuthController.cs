using InvoiceAPI.Models;
using InvoiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        // POST: api/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            var user = new User
            {
                UserName = newUser.UserName,
                Password = newUser.Password,
                RecordDate = DateTime.UtcNow
            };

            var result = await _userService.RegisterAsync(user);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }


          // POST: api/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginUser)
        {
            var user = new User
            {
                UserName = loginUser.UserName,
                Password = loginUser.Password
            };

            var result = await _userService.LoginAsync(user);

            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new { token = result.Message });
        }

        // GET: api/get-users
        [HttpGet("get-users")] // fatura oluşturuken kullanmak için kullanıcıları alıyoruz
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound(new { message = "Kullanıcılar bulunamadı." });
            }

            return Ok(users);
        }


          // GET: api/verify-token
        [HttpGet("verify-token")] // Token geçerli olup olmama endpoint'i
        public IActionResult VerifyToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.Replace("Bearer ", "");

            var isValid = _userService.VerifyToken(token);
            if (!isValid)
            {
                return Unauthorized(new { message = "Geçersiz token." });
            }

            return Ok(new { message = "Token geçerli." });
        }
    }
}
