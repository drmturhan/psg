using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.Migrations.Identity
{
    public partial class FotografLokalKayit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DosyaAdi",
                schema: "Polisomnografi",
                table: "KullaniciFotograflari",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosyaAdi",
                schema: "Polisomnografi",
                table: "KullaniciFotograflari");
        }
    }
}
