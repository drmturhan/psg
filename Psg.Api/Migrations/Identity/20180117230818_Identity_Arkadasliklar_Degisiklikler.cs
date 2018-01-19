using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.migrations.Identity
{
    public partial class Identity_Arkadasliklar_Degisiklikler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkadaslikTeklifleri_Kullanicilar_CevapVerenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri");

            migrationBuilder.RenameColumn(
                name: "CevapVerenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                newName: "TeklifEdilenNo");

            migrationBuilder.RenameIndex(
                name: "IX_ArkadaslikTeklifleri_CevapVerenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                newName: "IX_ArkadaslikTeklifleri_TeklifEdilenNo");

            migrationBuilder.AddForeignKey(
                name: "FK_ArkadaslikTeklifleri_Kullanicilar_TeklifEdilenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                column: "TeklifEdilenNo",
                principalSchema: "Polisomnografi",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkadaslikTeklifleri_Kullanicilar_TeklifEdilenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri");

            migrationBuilder.RenameColumn(
                name: "TeklifEdilenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                newName: "CevapVerenNo");

            migrationBuilder.RenameIndex(
                name: "IX_ArkadaslikTeklifleri_TeklifEdilenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                newName: "IX_ArkadaslikTeklifleri_CevapVerenNo");

            migrationBuilder.AddForeignKey(
                name: "FK_ArkadaslikTeklifleri_Kullanicilar_CevapVerenNo",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                column: "CevapVerenNo",
                principalSchema: "Polisomnografi",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
