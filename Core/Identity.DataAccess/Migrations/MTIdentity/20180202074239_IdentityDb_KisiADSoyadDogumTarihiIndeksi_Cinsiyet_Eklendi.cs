using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Identity.DataAccess.migrations.MTIdentity
{
    public partial class IdentityDb_KisiADSoyadDogumTarihiIndeksi_Cinsiyet_Eklendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "KisiAdSoyadSogumTarihiIndeks",
                schema: "Kisi",
                table: "Kisiler");

            migrationBuilder.CreateIndex(
                name: "KisiAdSoyadDogumTarihiCinsiyetIndeks",
                schema: "Kisi",
                table: "Kisiler",
                columns: new[] { "Ad", "Soyad", "DogumTarihi", "CinsiyetNo" },
                unique: true,
                filter: "[Ad] IS NOT NULL AND [Soyad] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "KisiAdSoyadDogumTarihiCinsiyetIndeks",
                schema: "Kisi",
                table: "Kisiler");

            migrationBuilder.CreateIndex(
                name: "KisiAdSoyadSogumTarihiIndeks",
                schema: "Kisi",
                table: "Kisiler",
                columns: new[] { "Ad", "Soyad", "DogumTarihi" },
                unique: true,
                filter: "[Ad] IS NOT NULL AND [Soyad] IS NOT NULL");
        }
    }
}
