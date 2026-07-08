using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SantiyeAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SantiyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public class LoginIstegi
        {
            public string KullaniciAdi { get; set; } = string.Empty;
            public string Sifre { get; set; } = string.Empty;
        }

        [HttpPost("Giris")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> GirisYapanAdamKim([FromBody] LoginIstegi form)
        {
            // Veritabanında bu kullanıcı adına sahip biri var mı bakıyoruz
            var adam = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == form.KullaniciAdi);

            // Şifre BCrypt hash'i ile karşılaştırılıyor (düz metin karşılaştırma yok)
            if (adam == null || !BCrypt.Net.BCrypt.Verify(form.Sifre, adam.Sifre))
            {
                return Unauthorized(new { mesaj = "Yanlış kullanıcı adı veya şifre girdin patron!" });
            }

            var token = TokenUret(adam);

            // Adamı bulduk! Kimliğini ve token'ı HTML tarafına gönderiyoruz.
            return Ok(new
            {
                mesaj = "Hoş geldin " + adam.AdSoyad,
                token,
                kullaniciId = adam.Id,
                adSoyad = adam.AdSoyad,
                rol = adam.Rol,
                firmaId = adam.Id
            });
        }

        private string TokenUret(Models.Kullanici adam)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, adam.Id.ToString()),
                new Claim(ClaimTypes.Name, adam.KullaniciAdi),
                new Claim(ClaimTypes.Role, adam.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireHours = _config.GetValue<int>("Jwt:ExpireHours", 24);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}