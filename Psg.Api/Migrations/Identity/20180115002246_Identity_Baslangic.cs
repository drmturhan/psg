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
                name: "Cinsiyetler",
                schema: "Polisomnografi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CinsiyetAdi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinsiyetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kisiler",
                schema: "Polisomnografi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(nullable: true),
                    CinsiyetNo = table.Column<int>(nullable: false),
                    DigerAd = table.Column<string>(nullable: true),
                    DogumTarihi = table.Column<DateTime>(nullable: false),
                    Soyad = table.Column<string>(nullable: true),
                    Unvan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kisiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kisiler_Cinsiyetler_CinsiyetNo",
                        column: x => x.CinsiyetNo,
                        principalSchema: "Polisomnografi",
                        principalTable: "Cinsiyetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                schema: "Polisomnografi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Aktif = table.Column<bool>(nullable: false),
                    EPosta = table.Column<string>(nullable: true),
                    EpostaOnaylandi = table.Column<bool>(nullable: true),
                    KisiNo = table.Column<int>(nullable: false),
                    KullaniciAdi = table.Column<string>(nullable: true),
                    SifreHash = table.Column<byte[]>(nullable: true),
                    SifreSalt = table.Column<byte[]>(nullable: true),
                    SonAktifOlma = table.Column<DateTime>(nullable: true),
                    TelefonNumarasi = table.Column<string>(nullable: true),
                    TelefonOnaylandi = table.Column<bool>(nullable: true),
                    YaratilmaTarihi = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_Kisiler_KisiNo",
                        column: x => x.KisiNo,
                        principalSchema: "Polisomnografi",
                        principalTable: "Kisiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciFotograflari",
                schema: "Polisomnografi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Aciklama = table.Column<string>(nullable: true),
                    EklenmeTarihi = table.Column<DateTime>(nullable: false),
                    IlkTercihmi = table.Column<bool>(nullable: false),
                    KullaniciNo = table.Column<int>(nullable: false),
                    PublicId = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciFotograflari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciFotograflari_Kullanicilar_KullaniciNo",
                        column: x => x.KullaniciNo,
                        principalSchema: "Polisomnografi",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kisiler_CinsiyetNo",
                schema: "Polisomnografi",
                table: "Kisiler",
                column: "CinsiyetNo");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciFotograflari_KullaniciNo",
                schema: "Polisomnografi",
                table: "KullaniciFotograflari",
                column: "KullaniciNo");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_KisiNo",
                schema: "Polisomnografi",
                table: "Kullanicilar",
                column: "KisiNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KullaniciFotograflari",
                schema: "Polisomnografi");

            migrationBuilder.DropTable(
                name: "Kullanicilar",
                schema: "Polisomnografi");

            migrationBuilder.DropTable(
                name: "Kisiler",
                schema: "Polisomnografi");

            migrationBuilder.DropTable(
                name: "Cinsiyetler",
                schema: "Polisomnografi");
        }
    }
}
