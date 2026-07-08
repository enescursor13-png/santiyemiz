namespace SantiyeAPI.Models;

/// <summary>
/// Bir işçi bir şantiyeden ayrılıp sonradan AYNI şantiyeye tekrar atandığında,
/// SantiyeIsci tablosundaki tek satır (composite key: IsciId+SantiyeId) üzerine
/// yeni dönem bilgileriyle yazılır. Bu tablo, üzerine yazılmadan önceki
/// (KatilmaTarihi, AyrilmaTarihi) dönemini kalıcı olarak arşivler ki
/// "bu işçi bu şantiyede ilk ne zaman çalışmaya başladı" bilgisi kaybolmasın.
/// </summary>
public class SantiyeIsciGecmisi
{
    public int Id { get; set; }
    public int IsciId { get; set; }
    public int SantiyeId { get; set; }
    public DateTime KatilmaTarihi { get; set; }
    public DateTime? AyrilmaTarihi { get; set; }
    public DateTime ArsivlenmeTarihi { get; set; }
}
