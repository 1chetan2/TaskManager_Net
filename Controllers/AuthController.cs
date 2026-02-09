using JwtApi.Data;
using JwtApi.DTOs;
using JwtApi.Models;
using JwtApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;

        public AuthController(
            AppDbContext context,
            IConfiguration config,
            EmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        // Register Process
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new Usertb
            {
                Email = dto.Email
            };

            var hasher = new PasswordHasher<Usertb>();
            user.PasswordHash = hasher.HashPassword(user, dto.Password);

            _context.Usertbs.Add(user);
            await _context.SaveChangesAsync();

            // Send welcome email (safe execution)
            try
            {
                await _emailService.SendEmailAsync(
                    dto.Email,
                    "Account Created",
                    "User account created successfully."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
            return Ok(new { message = "User registered successfully" });
        }


        // Login
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Usertbs.FirstOrDefault(x => x.Email == dto.Email);
            if (user == null) return Unauthorized();

            var hasher = new PasswordHasher<Usertb>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized();

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(Usertb user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
