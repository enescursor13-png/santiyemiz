using System;

namespace SantiyeAPI.Models;

public class Avans
{
    public int Id { get; set; }
    public DateTime Tarih { get; set; }
    public decimal Tutar { get; set; }
    public string OdemeTuru { get; set; } = "Nakit";
    public string? Aciklama { get; set; }

    // KİME verildi?
    public int IsciId { get; set; }
    public Isci? Isci { get; set; }

    // YENİ EKLENDİ: HANGİ şantiyenin hesabına yazılacak?
    public int SantiyeId { get; set; }
    public Santiye? Santiye { get; set; }

}
