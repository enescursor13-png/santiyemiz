namespace SantiyeAPI.Services;

using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Data;
using SantiyeAPI.DTOs;
using SantiyeAPI.Models;

public class IsciService : IIsciService
{
    private readonly AppDbContext _context;

    public IsciService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<IsciListDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Isciler.AsNoTracking();

        // 1. Önce toplam kayıt sayısını alıyoruz (Arayüzün bilmesi şart)
        var totalCount = await query.CountAsync(cancellationToken);


        // 2. Sadece istenen sayfanın verilerini çekiyoruz
        var items = await query
            .OrderByDescending(i => i.Id) // EKLENEN SATIR: En son eklenen en üstte çıksın
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new IsciListDto
            {
                Id = i.Id,
                Ad = i.Ad,
                Soyad = i.Soyad
            })
            .ToListAsync(cancellationToken);

        // 3. Veriyi ve üst bilgileri (Metadata) paketleyip gönderiyoruz
        return new PagedResult<IsciListDto>
        {
            Data = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IsciDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var isci = await _context.Isciler
            .AsNoTracking()
            .AsSplitQuery() // Çoka çok ve bire çok ilişkilerde performansı katlar
            .Include(i => i.Santiyeler)
            .Include(i => i.Avanslar)
            .Include(i => i.GunlukKayitlar)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (isci == null) return null;

        // Manuel Mapping (İleride AutoMapper kütüphanesi ile otomatikleştirilebilir)
        return new IsciDetailDto
        {
            Id = isci.Id,
            Ad = isci.Ad,
            Soyad = isci.Soyad,
            TcNo = isci.TcNo,
            GunlukUcret = isci.GunlukUcret,
            AktifMi = isci.AktifMi,
            Santiyeler = isci.Santiyeler.Select(s => new SantiyeBasitDto(s.Id, s.Ad)).ToList(),
            Avanslar = isci.Avanslar.Select(a => new AvansBasitDto(a.Id, a.Tarih, a.Tutar, a.OdemeTuru)).ToList(),
            GunlukKayitlar = isci.GunlukKayitlar.Select(g => new GunlukKayitBasitDto(g.Id, g.Tarih, g.CalismaKatsayisi, g.Yevmiye)).ToList()
        };
    }

    public async Task<IsciDetailDto> CreateAsync(IsciCreateDto createDto, CancellationToken cancellationToken)
    {
        var santiye = await _context.Santiyeler.FindAsync(new object[] { createDto.SantiyeId }, cancellationToken);

        if (santiye == null)
            throw new Exception("Seçilen şantiye bulunamadı!");

        // ------------------------------------------------------------------
        // SENIOR DOKUNUŞU: Gelen veriyi Türkçe kurallarına göre "Ütülüyoruz"
        // ------------------------------------------------------------------
        var culture = new CultureInfo("tr-TR"); // Türkçe dil kuralları
        var textInfo = culture.TextInfo;

        // Önce hepsini küçük harf yap, sonra her kelimenin ilk harfini büyüt ("aHMet cAn" -> "Ahmet Can")
        var formatliAd = textInfo.ToTitleCase(createDto.Ad.ToLower(culture));
        var formatliSoyad = textInfo.ToTitleCase(createDto.Soyad.ToLower(culture));
        var formatliMeslek = textInfo.ToTitleCase(createDto.Meslek.ToLower(culture));


        var yeniIsci = new Isci
        {
            Ad = formatliAd,         // Düzeltilmiş ismi veritabanına yolluyoruz
            Soyad = formatliSoyad,   // Düzeltilmiş soyismi yolluyoruz
            Meslek = formatliMeslek, // Düzeltilmiş mesleği yolluyoruz
            TcNo = createDto.TcNo,
            Telefon = createDto.Telefon,
            GunlukUcret = createDto.GunlukUcret,
            AktifMi = true,
            Santiyeler = new List<Santiye> { santiye }
        };

        _context.Isciler.Add(yeniIsci);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(yeniIsci.Id, cancellationToken) ?? throw new Exception("Kayıt hatası.");
    }

    public async Task<bool> UpdateAsync(int id, IsciUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var isci = await _context.Isciler.FindAsync(new object[] { id }, cancellationToken);
        if (isci == null) return false;

        isci.Ad = updateDto.Ad;
        isci.Soyad = updateDto.Soyad;
        isci.TcNo = updateDto.TcNo;
        isci.Telefon = updateDto.Telefon;
        isci.GunlukUcret = updateDto.GunlukUcret;
        isci.AktifMi = updateDto.AktifMi;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var isci = await _context.Isciler.FindAsync(new object[] { id }, cancellationToken);
        if (isci == null) return false;

        _context.Isciler.Remove(isci);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}