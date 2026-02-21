using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class KurumsalMimariVeIndeksleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avanslar_Santiyeler_SantiyeId",
                table: "Avanslar");

            migrationBuilder.DropForeignKey(
                name: "FK_GunlukKayitlar_Santiyeler_SantiyeId",
                table: "GunlukKayitlar");

            migrationBuilder.DropForeignKey(
                name: "FK_IsciSantiye_Isciler_IscilerId",
                table: "IsciSantiye");

            migrationBuilder.DropForeignKey(
                name: "FK_IsciSantiye_Santiyeler_SantiyelerId",
                table: "IsciSantiye");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IsciSantiye",
                table: "IsciSantiye");

            migrationBuilder.RenameTable(
                name: "IsciSantiye",
                newName: "SantiyeIsciBaglanti");

            migrationBuilder.RenameIndex(
                name: "IX_IsciSantiye_SantiyelerId",
                table: "SantiyeIsciBaglanti",
                newName: "IX_SantiyeIsciBaglanti_SantiyelerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "CalismaKatsayisi",
                table: "GunlukKayitlar",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SantiyeIsciBaglanti",
                table: "SantiyeIsciBaglanti",
                columns: new[] { "IscilerId", "SantiyelerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Santiyeler_Ad",
                table: "Santiyeler",
                column: "Ad");

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
                name: "IX_GunlukKayitlar_Tarih_SantiyeId",
                table: "GunlukKayitlar",
                columns: new[] { "Tarih", "SantiyeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_Tarih",
                table: "Avanslar",
                column: "Tarih");

            migrationBuilder.AddForeignKey(
                name: "FK_Avanslar_Santiyeler_SantiyeId",
                table: "Avanslar",
                column: "SantiyeId",
                principalTable: "Santiyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukKayitlar_Santiyeler_SantiyeId",
                table: "GunlukKayitlar",
                column: "SantiyeId",
                principalTable: "Santiyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyeIsciBaglanti_Isciler_IscilerId",
                table: "SantiyeIsciBaglanti",
                column: "IscilerId",
                principalTable: "Isciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyeIsciBaglanti_Santiyeler_SantiyelerId",
                table: "SantiyeIsciBaglanti",
                column: "SantiyelerId",
                principalTable: "Santiyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avanslar_Santiyeler_SantiyeId",
                table: "Avanslar");

            migrationBuilder.DropForeignKey(
                name: "FK_GunlukKayitlar_Santiyeler_SantiyeId",
                table: "GunlukKayitlar");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyeIsciBaglanti_Isciler_IscilerId",
                table: "SantiyeIsciBaglanti");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyeIsciBaglanti_Santiyeler_SantiyelerId",
                table: "SantiyeIsciBaglanti");

            migrationBuilder.DropIndex(
                name: "IX_Santiyeler_Ad",
                table: "Santiyeler");

            migrationBuilder.DropIndex(
                name: "IX_Isciler_Ad_Soyad",
                table: "Isciler");

            migrationBuilder.DropIndex(
                name: "IX_Isciler_TcNo",
                table: "Isciler");

            migrationBuilder.DropIndex(
                name: "IX_GunlukKayitlar_Tarih_SantiyeId",
                table: "GunlukKayitlar");

            migrationBuilder.DropIndex(
                name: "IX_Avanslar_Tarih",
                table: "Avanslar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SantiyeIsciBaglanti",
                table: "SantiyeIsciBaglanti");

            migrationBuilder.RenameTable(
                name: "SantiyeIsciBaglanti",
                newName: "IsciSantiye");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyeIsciBaglanti_SantiyelerId",
                table: "IsciSantiye",
                newName: "IX_IsciSantiye_SantiyelerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "CalismaKatsayisi",
                table: "GunlukKayitlar",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IsciSantiye",
                table: "IsciSantiye",
                columns: new[] { "IscilerId", "SantiyelerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Avanslar_Santiyeler_SantiyeId",
                table: "Avanslar",
                column: "SantiyeId",
                principalTable: "Santiyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukKayitlar_Santiyeler_SantiyeId",
                table: "GunlukKayitlar",
                column: "SantiyeId",
                principalTable: "Santiyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IsciSantiye_Isciler_IscilerId",
                table: "IsciSantiye",
                column: "IscilerId",
                principalTable: "Isciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IsciSantiye_Santiyeler_SantiyelerId",
                table: "IsciSantiye",
                column: "SantiyelerId",
                principalTable: "Santiyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
