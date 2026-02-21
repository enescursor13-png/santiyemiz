using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Data;
// Kendi model (Entity) klasörünün adını buraya eklemeyi unutma (Örn: using SantiyeAPI.Models; veya Entities)

namespace SantiyeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PuantajController : ControllerBase
{
    private readonly AppDbContext _context;

    public PuantajController(AppDbContext context)
    {
        _context = context;
    }

    // Dışarıdan gelecek olan Hızlı Yoklama verisinin iskeleti (DTO)
    public class HizliPuantajDto
    {
        public int IsciId { get; set; }
        public int SantiyeId { get; set; }
        public double Katsayi { get; set; } // 1.0 (Tam), 0.5 (Yarım), 0.0 (Gelmedi)
        public string Tarih { get; set; } = string.Empty; // "2026-02-21" formatında gelecek
    }

    [HttpPost("hizli-kayit")]
    public async Task<IActionResult> HizliKayit([FromBody] HizliPuantajDto dto)
    {
        // 1. İşçiyi bul (Maaşını öğrenmek için)
        var isci = await _context.Isciler.FindAsync(dto.IsciId);
        if (isci == null) return NotFound("İşçi bulunamadı!");

        var bugun = DateTime.Parse(dto.Tarih).Date; // Sadece gün-ay-yıl kısmını alıyoruz

        // 2. SENIOR KORUMASI: Bu adamın bugün bu şantiyede zaten bir kaydı var mı?
        var eskiKayit = await _context.GunlukKayitlar
            .FirstOrDefaultAsync(g => g.IsciId == dto.IsciId && g.SantiyeId == dto.SantiyeId && g.Tarih.Date == bugun);

        // Hesaplanacak net yevmiye
        decimal hakEdis = (decimal)((double)isci.GunlukUcret * dto.Katsayi);
        string aciklama = dto.Katsayi == 1.0 ? "Tam Gün" : (dto.Katsayi == 0.5 ? "Yarım Gün" : "Gelmedi");

        if (eskiKayit != null)
        {
            // Kayıt varsa ÜZERİNE YAZ (Update)
            eskiKayit.CalismaKatsayisi = (decimal)dto.Katsayi;
            eskiKayit.Yevmiye = hakEdis;
            eskiKayit.Aciklama = aciklama;
        }
        else
        {
            // Kayıt yoksa YENİ OLUŞTUR (Insert)
            // Not: Senin Entity sınıfının adı GunlukKayit veya Puantaj olabilir, ona göre düzeltirsin.
            var yeniKayit = new SantiyeAPI.Models.GunlukKayit // <-- Model adın farklıysa burayı düzelt
            {
                IsciId = dto.IsciId,
                SantiyeId = dto.SantiyeId,
                Tarih = bugun,
                CalismaKatsayisi = (decimal)dto.Katsayi,
                Yevmiye = hakEdis,
                Aciklama = aciklama
            };
            _context.GunlukKayitlar.Add(yeniKayit);
        }

        await _context.SaveChangesAsync();

        return Ok(new { Mesaj = $"{isci.Ad} için {aciklama} ({hakEdis} ₺) hesaba işlendi." });
    }
    // SENIOR TELAFİSİ: Şefin takvimden seçtiği günün puantaj geçmişini getirir
    [HttpGet("santiye/{santiyeId}/tarih/{tarih}")]
    public async Task<IActionResult> GetGunlukKayitlar(int santiyeId, string tarih)
    {
        var seciliTarih = DateTime.Parse(tarih).Date;

        var kayitlar = await _context.GunlukKayitlar
            .AsNoTracking()
            .Where(g => g.SantiyeId == santiyeId && g.Tarih.Date == seciliTarih)
            .Select(g => new 
            { 
                g.IsciId, 
                g.CalismaKatsayisi 
            })
            .ToListAsync();

        return Ok(kayitlar);
    }
}