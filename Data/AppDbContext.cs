using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Models;
using System.Reflection; // Assembly sihrini kullanmak için şart!

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ====================================================================
        // SENIOR MİMARİ: TEK SATIRDA TÜM KURALLARI YÜKLE
        // ====================================================================
        // Bu kod parçası gider; IsciConfiguration, SantiyeConfiguration, AvansConfiguration
        // gibi "IEntityTypeConfiguration"dan miras alan ne kadar dosya varsa 
        // bulur ve otomatik olarak veritabanına uygular. 
        // Onlarca satır kod yazmaktan bizi kurtarır, hata payını sıfıra indirir.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // ====================================================================
        // DATA SEEDING (Sistemi Hazır Veriyle Başlatma)
        // ====================================================================
        // Baban uygulamayı ilk açtığında bomboş bir sayfa görmesin diye
        // temel şantiyeleri buraya mühürlüyoruz.
        modelBuilder.Entity<Santiye>().HasData(
            new Santiye { Id = 1, Ad = "A Şantiyesi (Merkez)", Konum = "İstanbul", AktifMi = true },
            new Santiye { Id = 2, Ad = "B Şantiyesi (Toki Konutları)", Konum = "Ankara", AktifMi = true }
        );
    }
}