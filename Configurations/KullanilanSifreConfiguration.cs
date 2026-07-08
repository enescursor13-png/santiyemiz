namespace SantiyeAPI.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SantiyeAPI.Models;

public class KullanilanSifreConfiguration : IEntityTypeConfiguration<KullanilanSifre>
{
    public void Configure(EntityTypeBuilder<KullanilanSifre> builder)
    {
        builder.HasKey(k => k.Id);

        // 🚀 ÇİFT KULLANIM ZIRHI: Aynı firma için aynı jeton kodu veritabanı
        // seviyesinde sadece bir kez kaydedilebilir. İki eşzamanlı istek aynı
        // kodu göndermeye çalışırsa, ikincisi bu kısıt sayesinde reddedilir
        // (uygulama kodundaki "daha önce kullanıldı mı" kontrolüne güvenmek yerine).
        builder.HasIndex(k => new { k.CompanyId, k.Sifre })
               .IsUnique()
               .HasDatabaseName("UX_KullanilanSifre_TekKullanimZirhi");
    }
}
