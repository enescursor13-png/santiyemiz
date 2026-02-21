using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeAPI.Migrations
{
    /// <inheritdoc />
    public partial class IsciMeslekEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meslek",
                table: "Isciler",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meslek",
                table: "Isciler");
        }
    }
}
