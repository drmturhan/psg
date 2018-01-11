using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.Migrations.Identity
{
    public partial class Identity_Baslangic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Polisomnografi");

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                schema: "Polisomnografi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(nullable: true),
                    Aktif = table.Column<bool>(nullable: false),
                    Cinsiyeti = table.Column<string>(nullable: true),
                    DigerAd = table.Column<string>(nullable: true),
                    DogumTarihi = table.Column<DateTime>(nullable: false),
                    EPosta = table.Column<string>(nullable: true),
                    EpostaOnaylandi = table.Column<bool>(nullable: true),
                    KullaniciAdi = table.Column<string>(nullable: true),
                    SifreHash = table.Column<byte[]>(nullable: true),
                    SifreSalt = table.Column<byte[]>(nullable: true),
                    SonAktifOlma = table.Column<DateTime>(nullable: true),
                    Soyad = table.Column<string>(nullable: true),
                    TelefonNumarasi = table.Column<string>(nullable: true),
                    TelefonOnaylandi = table.Column<bool>(nullable: true),
                    Unvan = table.Column<string>(nullable: true),
                    YaratilmaTarihi = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fotograflar",
                schema: "Polisomnografi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Aciklama = table.Column<string>(nullable: true),
                    EklenmeTarihi = table.Column<DateTime>(nullable: false),
                    IlkTercihmi = table.Column<bool>(nullable: false),
                    KullaniciNo = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotograflar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotograflar_Kullanicilar_KullaniciNo",
                        column: x => x.KullaniciNo,
                        principalSchema: "Polisomnografi",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotograflar_KullaniciNo",
                schema: "Polisomnografi",
                table: "Fotograflar",
                column: "KullaniciNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotograflar",
                schema: "Polisomnografi");

            migrationBuilder.DropTable(
                name: "Kullanicilar",
                schema: "Polisomnografi");
        }
    }
}
