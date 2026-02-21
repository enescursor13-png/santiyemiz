using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SantiyeAPI.Models;

namespace SantiyeAPI.Configurations;

public class AvansConfiguration : IEntityTypeConfiguration<Avans>
{
    public void Configure(EntityTypeBuilder<Avans> builder)
    {
        builder.ToTable("Avanslar");
        builder.HasKey(a => a.Id);

        // 1. Kolon Özellikleri
        builder.Property(a => a.Tutar)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(a => a.OdemeTuru)
               .IsRequired()
               .HasMaxLength(30); // Nakit, Banka, Elden vb.

        builder.Property(a => a.Aciklama)
               .HasMaxLength(500); 

        // 2. 🚀 PERFORMANS VE RAPORLAMA İNDEKSLERİ
        
        // Ay sonunda bir işçinin toplam ne kadar avans aldığını hesaplarken (HAKEDİŞ HESABI)
        // motorun tüm tabloyu taramaması için IsciId üzerine indeks çakıyoruz.
        builder.HasIndex(a => a.IsciId);

        // Belirli bir tarihteki kasa çıkışlarını görmek için Tarih indeksi
        builder.HasIndex(a => a.Tarih);

        // 3. 🔗 İLİŞKİ YAPILANDIRMASI
        
        builder.HasOne(a => a.Isci)
               .WithMany(i => i.Avanslar)
               .HasForeignKey(a => a.IsciId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Santiye)
               .WithMany() 
               .HasForeignKey(a => a.SantiyeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}