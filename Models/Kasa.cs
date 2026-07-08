using SantiyeAPI.Helpers;

namespace SantiyeAPI.Models;

public class Kasa
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty; // Ahmet Bey, Merkez Şantiye vb.
    public bool AktifMi { get; set; } = true;
    public DateTime OlusturulmaTarihi { get; set; } = ZamanMotoru.SimdiTurkiye();

    //public decimal Bakiye { get; set; } = 0;

    // Navigation (Bir kasanın birden fazla hareketi olur)
    public ICollection<KasaHareketi> Hareketler { get; set; } = new List<KasaHareketi>();
}