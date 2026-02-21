using System;

namespace SantiyeAPI.Models;

public class Isci
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string TcNo { get; set; } = string.Empty;
    public string Meslek { get; set; } = string.Empty; // SENIOR DOKUNUŞU: Kalıpçı, Demirci vs.
    public string Telefon { get; set; } = string.Empty;

    public decimal GunlukUcret { get; set; }
    public bool AktifMi { get; set; } = true;

    // DÜZELTİLDİ: Artık tek bir SantiyeId yok! Bir işçinin BİRDEN FAZLA şantiyesi olabilir.
    public List<Santiye> Santiyeler { get; set; } = new List<Santiye>();

    public List<GunlukKayit> GunlukKayitlar { get; set; } = new List<GunlukKayit>();
    public List<Avans> Avanslar { get; set; } = new List<Avans>();
}
