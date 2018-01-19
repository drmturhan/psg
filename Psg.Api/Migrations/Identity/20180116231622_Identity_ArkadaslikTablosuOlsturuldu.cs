using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.Migrations.Identity
{
    public partial class Identity_ArkadaslikTablosuOlsturuldu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArkadaslikTeklifleri",
                schema: "Polisomnografi",
                columns: table => new
                {
                    ArkadaslikIsteyenNo = table.Column<int>(nullable: false),
                    CevapVerenNo = table.Column<int>(nullable: false),
                    CevapTarihi = table.Column<DateTime>(nullable: true),
                    IstekTarihi = table.Column<DateTime>(nullable: false),
                    Karar = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArkadaslikTeklifleri", x => new { x.ArkadaslikIsteyenNo, x.CevapVerenNo });
                    table.ForeignKey(
                        name: "FK_ArkadaslikTeklifleri_Kullanicilar_ArkadaslikIsteyenNo",
                        column: x => x.ArkadaslikIsteyenNo,
                        principalSchema: "Polisomnografi",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArkadaslikTeklifleri_Kullanicilar_CevapVerenNo",
                        column: x => x.CevapVerenNo,
                        principalSchema: "Polisomnografi",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArkadaslikTeklifleri_CevapVerenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                column: "CevapVerenNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArkadaslikTeklifleri",
                schema: "Polisomnografi");
        }
    }
}
