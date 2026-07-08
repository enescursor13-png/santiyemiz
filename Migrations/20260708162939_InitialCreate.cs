using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SantiyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AllowedActiveSiteCount = table.Column<int>(type: "integer", nullable: false),
                    DonanimKimligi = table.Column<string>(type: "text", nullable: true),
                    SonIslemTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DamgaHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HarcamaKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarcamaKategorileri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Isciler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TcNo = table.Column<string>(type: "character(11)", fixedLength: true, maxLength: 11, nullable: false),
                    Meslek = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GunlukUcret = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Isciler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kasalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kasalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdSoyad = table.Column<string>(type: "text", nullable: false),
                    KullaniciAdi = table.Column<string>(type: "text", nullable: false),
                    Sifre = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SantiyeIsciGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsciId = table.Column<int>(type: "integer", nullable: false),
                    SantiyeId = table.Column<int>(type: "integer", nullable: false),
                    KatilmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AyrilmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ArsivlenmeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyeIsciGecmisleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SatinAlmaGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlinanJetonSayisi = table.Column<int>(type: "integer", nullable: false),
                    OdenenTutar = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatinAlmaGecmisleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KullanilanSifreler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Sifre = table.Column<string>(type: "text", nullable: false),
                    KullanımTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullanilanSifreler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullanilanSifreler_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Santiyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Konum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    LisansBitisTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santiyeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santiyeler_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaasGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsciId = table.Column<int>(type: "integer", nullable: true),
                    Yevmiye = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaasGecmisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaasGecmisleri_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avanslar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OdemeTuru = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OdendiMi = table.Column<bool>(type: "boolean", nullable: false),
                    IsciId = table.Column<int>(type: "integer", nullable: false),
                    SantiyeId = table.Column<int>(type: "integer", nullable: true),
                    KasaId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avanslar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avanslar_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avanslar_Kasalar_KasaId",
                        column: x => x.KasaId,
                        principalTable: "Kasalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avanslar_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GunlukKayitlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CalismaKatsayisi = table.Column<decimal>(type: "numeric(3,2)", nullable: false),
                    Yevmiye = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    OdendiMi = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsciId = table.Column<int>(type: "integer", nullable: false),
                    SantiyeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GunlukKayitlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GunlukKayitlar_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GunlukKayitlar_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaasOdemeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsciId = table.Column<int>(type: "integer", nullable: false),
                    KasaId = table.Column<int>(type: "integer", nullable: false),
                    SantiyeId = table.Column<int>(type: "integer", nullable: true),
                    Tutar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IslemTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaasOdemeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaasOdemeleri_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaasOdemeleri_Kasalar_KasaId",
                        column: x => x.KasaId,
                        principalTable: "Kasalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaasOdemeleri_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patronlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    Unvan = table.Column<string>(type: "text", nullable: false),
                    KasaId = table.Column<int>(type: "integer", nullable: true),
                    SorumluOlduguSantiyeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patronlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patronlar_Kasalar_KasaId",
                        column: x => x.KasaId,
                        principalTable: "Kasalar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patronlar_Santiyeler_SorumluOlduguSantiyeId",
                        column: x => x.SorumluOlduguSantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SantiyeIsciler",
                columns: table => new
                {
                    IsciId = table.Column<int>(type: "integer", nullable: false),
                    SantiyeId = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    KatilmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AyrilmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyeIsciler", x => new { x.IsciId, x.SantiyeId });
                    table.ForeignKey(
                        name: "FK_SantiyeIsciler_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SantiyeIsciler_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SantiyeNotlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SantiyeId = table.Column<int>(type: "integer", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NotMetni = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyeNotlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SantiyeNotlari_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KasaHareketleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KasaId = table.Column<int>(type: "integer", nullable: false),
                    SantiyeId = table.Column<int>(type: "integer", nullable: true),
                    Tutar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Yon = table.Column<int>(type: "integer", nullable: false),
                    HareketTipi = table.Column<int>(type: "integer", nullable: false),
                    ReferansTabloAdi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReferansId = table.Column<int>(type: "integer", nullable: true),
                    TransferGrupId = table.Column<Guid>(type: "uuid", nullable: true),
                    IptalEdilenIslemId = table.Column<int>(type: "integer", nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IslemTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    PatronId = table.Column<int>(type: "integer", nullable: true),
                    SifirlamaFisiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KasaHareketleri", x => x.Id);
                    table.CheckConstraint("CK_KasaHareketi_Tutar_Pozitif", "\"Tutar\" > 0");
                    table.ForeignKey(
                        name: "FK_KasaHareketleri_KasaHareketleri_IptalEdilenIslemId",
                        column: x => x.IptalEdilenIslemId,
                        principalTable: "KasaHareketleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KasaHareketleri_Kasalar_KasaId",
                        column: x => x.KasaId,
                        principalTable: "Kasalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KasaHareketleri_Patronlar_PatronId",
                        column: x => x.PatronId,
                        principalTable: "Patronlar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KasaHareketleri_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "Id", "AdSoyad", "KullaniciAdi", "Rol", "Sifre" },
                values: new object[] { 1, "Muhammet Zeki", "sword", "Sef", "$2a$11$ZC1QVdefW1vTqeUPdPcEvOdbuoTy9xY7fvR/hOEnmzWJ1e.5fgxJu" });

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_IsciId",
                table: "Avanslar",
                column: "IsciId");

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_KasaId",
                table: "Avanslar",
                column: "KasaId");

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_SantiyeId",
                table: "Avanslar",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_Tarih_OdendiMi",
                table: "Avanslar",
                columns: new[] { "Tarih", "OdendiMi" });

            migrationBuilder.CreateIndex(
                name: "IX_GunlukKayitlar_IsciId",
                table: "GunlukKayitlar",
                column: "IsciId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukKayitlar_SantiyeId_Tarih_OdendiMi",
                table: "GunlukKayitlar",
                columns: new[] { "SantiyeId", "Tarih", "OdendiMi" });

            migrationBuilder.CreateIndex(
                name: "UX_GunlukKayit_TekKayitZirhi",
                table: "GunlukKayitlar",
                columns: new[] { "IsciId", "SantiyeId", "Tarih" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Isciler_Ad_Soyad",
                table: "Isciler",
                columns: new[] { "Ad", "Soyad" });

            migrationBuilder.CreateIndex(
                name: "IX_Isciler_TcNo",
                table: "Isciler",
                column: "TcNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareket_Referans",
                table: "KasaHareketleri",
                columns: new[] { "ReferansTabloAdi", "ReferansId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareketleri_IptalEdilenIslemId",
                table: "KasaHareketleri",
                column: "IptalEdilenIslemId");

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareketleri_KasaId_IslemTarihi",
                table: "KasaHareketleri",
                columns: new[] { "KasaId", "IslemTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareketleri_PatronId",
                table: "KasaHareketleri",
                column: "PatronId");

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareketleri_SantiyeId",
                table: "KasaHareketleri",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "UX_KullanilanSifre_TekKullanimZirhi",
                table: "KullanilanSifreler",
                columns: new[] { "CompanyId", "Sifre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaasGecmisleri_IsciId_BaslangicTarihi",
                table: "MaasGecmisleri",
                columns: new[] { "IsciId", "BaslangicTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_MaasOdemeleri_IsciId",
                table: "MaasOdemeleri",
                column: "IsciId");

            migrationBuilder.CreateIndex(
                name: "IX_MaasOdemeleri_KasaId",
                table: "MaasOdemeleri",
                column: "KasaId");

            migrationBuilder.CreateIndex(
                name: "IX_MaasOdemeleri_SantiyeId",
                table: "MaasOdemeleri",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patronlar_KasaId",
                table: "Patronlar",
                column: "KasaId");

            migrationBuilder.CreateIndex(
                name: "IX_Patronlar_SorumluOlduguSantiyeId",
                table: "Patronlar",
                column: "SorumluOlduguSantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_SantiyeIsciler_IsciId_AktifMi",
                table: "SantiyeIsciler",
                columns: new[] { "IsciId", "AktifMi" });

            migrationBuilder.CreateIndex(
                name: "IX_SantiyeIsciler_SantiyeId",
                table: "SantiyeIsciler",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Santiyeler_Ad",
                table: "Santiyeler",
                column: "Ad");

            migrationBuilder.CreateIndex(
                name: "IX_Santiyeler_CompanyId",
                table: "Santiyeler",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SantiyeNotlari_SantiyeId",
                table: "SantiyeNotlari",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_SantiyeNotlari_Tarih",
                table: "SantiyeNotlari",
                column: "Tarih");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avanslar");

            migrationBuilder.DropTable(
                name: "GunlukKayitlar");

            migrationBuilder.DropTable(
                name: "HarcamaKategorileri");

            migrationBuilder.DropTable(
                name: "KasaHareketleri");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "KullanilanSifreler");

            migrationBuilder.DropTable(
                name: "MaasGecmisleri");

            migrationBuilder.DropTable(
                name: "MaasOdemeleri");

            migrationBuilder.DropTable(
                name: "SantiyeIsciGecmisleri");

            migrationBuilder.DropTable(
                name: "SantiyeIsciler");

            migrationBuilder.DropTable(
                name: "SantiyeNotlari");

            migrationBuilder.DropTable(
                name: "SatinAlmaGecmisleri");

            migrationBuilder.DropTable(
                name: "Patronlar");

            migrationBuilder.DropTable(
                name: "Isciler");

            migrationBuilder.DropTable(
                name: "Kasalar");

            migrationBuilder.DropTable(
                name: "Santiyeler");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
