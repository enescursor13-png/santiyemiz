using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SantiyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class YeniKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Isciler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", nullable: false),
                    Soyad = table.Column<string>(type: "TEXT", nullable: false),
                    TcNo = table.Column<string>(type: "TEXT", nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", nullable: false),
                    GunlukUcret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Isciler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Santiyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", nullable: false),
                    Konum = table.Column<string>(type: "TEXT", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santiyeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avanslar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OdemeTuru = table.Column<string>(type: "TEXT", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", nullable: true),
                    IsciId = table.Column<int>(type: "INTEGER", nullable: false),
                    SantiyeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avanslar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avanslar_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avanslar_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GunlukKayitlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CalismaKatsayisi = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Yevmiye = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", nullable: true),
                    IsciId = table.Column<int>(type: "INTEGER", nullable: false),
                    SantiyeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GunlukKayitlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GunlukKayitlar_Isciler_IsciId",
                        column: x => x.IsciId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GunlukKayitlar_Santiyeler_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IsciSantiye",
                columns: table => new
                {
                    IscilerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SantiyelerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsciSantiye", x => new { x.IscilerId, x.SantiyelerId });
                    table.ForeignKey(
                        name: "FK_IsciSantiye_Isciler_IscilerId",
                        column: x => x.IscilerId,
                        principalTable: "Isciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IsciSantiye_Santiyeler_SantiyelerId",
                        column: x => x.SantiyelerId,
                        principalTable: "Santiyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Santiyeler",
                columns: new[] { "Id", "Ad", "AktifMi", "Konum" },
                values: new object[,]
                {
                    { 1, "A Şantiyesi (Merkez)", true, "İstanbul" },
                    { 2, "B Şantiyesi (Toki Konutları)", true, "Ankara" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_IsciId",
                table: "Avanslar",
                column: "IsciId");

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_SantiyeId",
                table: "Avanslar",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukKayitlar_IsciId",
                table: "GunlukKayitlar",
                column: "IsciId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukKayitlar_SantiyeId",
                table: "GunlukKayitlar",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_IsciSantiye_SantiyelerId",
                table: "IsciSantiye",
                column: "SantiyelerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avanslar");

            migrationBuilder.DropTable(
                name: "GunlukKayitlar");

            migrationBuilder.DropTable(
                name: "IsciSantiye");

            migrationBuilder.DropTable(
                name: "Isciler");

            migrationBuilder.DropTable(
                name: "Santiyeler");
        }
    }
}
