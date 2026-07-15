using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics; // Assembly sihrini kullanmak için şart!

namespace SantiyeAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<Isci> Isciler { get; set; }
    public DbSet<Santiye> Santiyeler { get; set; }
    public DbSet<GunlukKayit> GunlukKayitlar { get; set; }
    public DbSet<Avans> Avanslar { get; set; }
    public DbSet<SantiyeIsci> SantiyeIsciler { get; set; }
    public DbSet<SantiyeIsciGecmisi> SantiyeIsciGecmisleri { get; set; }
    public DbSet<MaasGecmisi> MaasGecmisleri { get; set; }
    // 🚀 YENİ EKLENEN FİNANS / KASA TABLOLARI (Eksik olan kısım burası!)
    public DbSet<Kasa> Kasalar { get; set; }
    public DbSet<KasaHareketi> KasaHareketleri { get; set; }
    public DbSet<HarcamaKategori> HarcamaKategorileri { get; set; }
    // İçine şu satırı mutlaka ekle:
    public DbSet<MaasOdemesi> MaasOdemeleri { get; set; }
    public DbSet<Patron> Patronlar { get; set; }
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<SantiyeNotu> SantiyeNotlari { get; set; }

    public DbSet<Company> Companies { get; set; }
    public DbSet<SatinAlmaGecmisi> SatinAlmaGecmisleri { get; set; }

    public DbSet<KullanilanSifre> KullanilanSifreler { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // ====================================================================
        // SENIOR MİMARİ: TEK SATIRDA TÜM KURALLARI YÜKLE
        // 
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Kullanici>().HasData(
        new Kullanici
        {
            Id = 1,
            KullaniciAdi = "ahmetfurkan",
            Sifre = "$2a$11$p.PApCHcjKiXJn8E3SflfOxlwxhcyYAEHr2ExFokjknFeJXaRuLY2", // bcrypt("sahin_38")
            Rol = "Sef",
            AdSoyad = "Muhammet Zeki"
        }
    );

        // 🐘 POSTGRESQL UYUMLULUĞU: Uygulama tüm tarihleri Türkiye yerel saati olarak
        // (ZamanMotoru.SimdiTurkiye(), Kind=Unspecified) tutuyor — UTC değil. SQLite bu
        // konuda esnekti ama Npgsql varsayılan olarak "timestamp with time zone" sütununa
        // sadece Kind=Utc DateTime yazılmasına izin veriyor. Burada tüm DateTime
        // sütunlarını "timestamp without time zone" (saat dilimsiz, ham) olarak
        // işaretliyoruz ki mevcut iş mantığı (ay/gün karşılaştırmaları vb.) değişmeden
        // çalışmaya devam etsin.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp without time zone");
                }
            }
        }
    }
}