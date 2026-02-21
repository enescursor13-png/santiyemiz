namespace SantiyeAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using SantiyeAPI.DTOs;
using SantiyeAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class IsciController : ControllerBase
{
    private readonly IIsciService _isciService;

    public IsciController(IIsciService isciService)
    {
        _isciService = isciService;
    }

    /// <summary>
    /// İşçileri sayfalayarak hafif DTO formatında listeler.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<IsciListDto>>> GetAllAsync(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var isciler = await _isciService.GetAllAsync(pageNumber, pageSize, cancellationToken);
        return Ok(isciler);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip işçinin tüm detaylarını (Avans, Puantaj vb.) getirir.
    /// </summary>
    [HttpGet("{id}", Name = "GetIsciById")] // Bu adrese sarsılmaz bir isim taktık
    public async Task<ActionResult<IsciDetailDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var isci = await _isciService.GetByIdAsync(id, cancellationToken);
        if (isci == null) return NotFound(new { Mesaj = $"ID'si {id} olan işçi bulunamadı." });

        return Ok(isci);
    }

    /// <summary>
    /// Yeni bir işçi kaydı oluşturur.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<IsciDetailDto>> CreateAsync(
        [FromBody] IsciCreateDto createDto,
        CancellationToken cancellationToken)
    {
        var olusturulanIsci = await _isciService.CreateAsync(createDto, cancellationToken);

        // 201 Created döner ve Response Header'a yeni kaynağın URI adresini ekler.
        // Artık o isimlendirdiğimiz sarsılmaz adresi çağırıyoruz
        return CreatedAtRoute("GetIsciById", new { id = olusturulanIsci.Id }, olusturulanIsci);
    }

    /// <summary>
    /// Mevcut bir işçinin bilgilerini günceller.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] IsciUpdateDto updateDto,
        CancellationToken cancellationToken)
    {
        var sonuc = await _isciService.UpdateAsync(id, updateDto, cancellationToken);
        if (!sonuc) return NotFound(new { Mesaj = $"Güncellenecek işçi bulunamadı (ID: {id})." });

        return NoContent(); // 204 Başarılı ama dönülecek veri yok
    }

    /// <summary>
    /// Belirtilen işçiyi sistemden siler.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var sonuc = await _isciService.DeleteAsync(id, cancellationToken);
        if (!sonuc) return NotFound(new { Mesaj = $"Silinecek işçi bulunamadı (ID: {id})." });

        return NoContent();
    }
}