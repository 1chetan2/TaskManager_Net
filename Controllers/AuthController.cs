using JwtApi.Data;
using JwtApi.DTOs;
using JwtApi.Models;
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

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Register Process
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new Usertb
            {
                Email = dto.Email
            };
            var hasher = new PasswordHasher<Usertb>();
            user.PasswordHash = hasher.HashPassword(user, dto.Password);

            _context.Usertbs.Add(user);
            _context.SaveChanges();

            return Ok("User Registered");
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

