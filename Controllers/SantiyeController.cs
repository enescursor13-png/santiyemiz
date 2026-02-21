using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Data;

namespace SantiyeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SantiyeController : ControllerBase
{
    private readonly AppDbContext _context;

    // Veritabanı bağlantımızı içeri alıyoruz
    public SantiyeController(AppDbContext context)
    {
        _context = context;
    }

    // 1. ANA EKRAN İÇİN: Tüm Şantiyeleri ve içindeki anlık işçi sayısını getirir
    [HttpGet]
    public async Task<IActionResult> GetSantiyeler()
    {
        var santiyeler = await _context.Santiyeler
            .AsNoTracking() // Sadece okuyacağız, EF Core yorulmasın
            .Select(s => new
            {
                s.Id,
                s.Ad,
                s.Konum,
                s.AktifMi,
                IsciSayisi = s.Isciler.Count(i => i.AktifMi == true) // Şov kısmı: O şantiyede kaç kişi var anında sayar!
            })
            .ToListAsync();

        return Ok(santiyeler);
    }

    // 2. ŞANTİYE İÇİNE GİRİNCE: Sadece o şantiyeye kayıtlı olan işçileri getirir
    [HttpGet("{santiyeId}/isciler")]
    public async Task<IActionResult> GetIscilerBySantiye(int santiyeId)
    {
        var isciler = await _context.Isciler
            .AsNoTracking()
            .Where(i => i.Santiyeler.Any(s => s.Id == santiyeId) && i.AktifMi == true)
            .Select(i => new
            {
                i.Id,
                i.Ad, 
                i.Soyad,
                i.Meslek,
                i.TcNo,
                i.Telefon,
                i.GunlukUcret
            })
            .ToListAsync();

        return Ok(isciler);
    }
    // SENIOR DOKUNUŞU: İşçiyi silmiyoruz, pasife çekiyoruz ki muhasebesi bozulmasın!
    [HttpPut("isci-cikar/{isciId}")]
    public async Task<IActionResult> IsciCikar(int isciId)
    {
        var isci = await _context.Isciler.FindAsync(isciId);
        if (isci == null) return NotFound("İşçi bulunamadı!");

        isci.AktifMi = false; // İşten çıkardık (Pasife aldık)
        await _context.SaveChangesAsync();

        return Ok(new { Mesaj = $"{isci.Ad} adlı işçinin şantiye ile ilişiği kesildi." });
    }
    // SENIOR DOKUNUŞU: Dışarıdan gelecek Şantiye verisi için minik bir kurye paketi (DTO)
    public class SantiyeCreateDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Konum { get; set; } = string.Empty;
    }

    // YENİ ŞANTİYE EKLEME UCU
    [HttpPost]
    public async Task<IActionResult> CreateSantiye([FromBody] SantiyeCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Ad))
            return BadRequest("Şantiye adı boş olamaz!");

        // Gelen veriyi Veritabanı modelimize çeviriyoruz
        var yeniSantiye = new SantiyeAPI.Models.Santiye // Not: Model adın veya namespace'in farklıysa 'Santiye' kısmını düzelt
        {
            Ad = dto.Ad,
            Konum = dto.Konum,
            AktifMi = true
        };

        _context.Santiyeler.Add(yeniSantiye);
        await _context.SaveChangesAsync();

        return Ok(new { Mesaj = "Şantiye başarıyla oluşturuldu." });
    }
}