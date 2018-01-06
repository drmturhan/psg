using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.Migrations.Identity
{
    public partial class UykuTestiHastaIliskisiTanimlandi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UykuTestleri_Hastalar_HastaId",
                table: "UykuTestleri");

            migrationBuilder.DropIndex(
                name: "IX_UykuTestleri_HastaId",
                table: "UykuTestleri");

            migrationBuilder.DropColumn(
                name: "HastaId",
                table: "UykuTestleri");

            migrationBuilder.CreateIndex(
                name: "IX_UykuTestleri_HastaNo",
                table: "UykuTestleri",
                column: "HastaNo");

            migrationBuilder.AddForeignKey(
                name: "FK_UykuTestleri_Hastalar_HastaNo",
                table: "UykuTestleri",
                column: "HastaNo",
                principalTable: "Hastalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UykuTestleri_Hastalar_HastaNo",
                table: "UykuTestleri");

            migrationBuilder.DropIndex(
                name: "IX_UykuTestleri_HastaNo",
                table: "UykuTestleri");

            migrationBuilder.AddColumn<int>(
                name: "HastaId",
                table: "UykuTestleri",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UykuTestleri_HastaId",
                table: "UykuTestleri",
                column: "HastaId");

            migrationBuilder.AddForeignKey(
                name: "FK_UykuTestleri_Hastalar_HastaId",
                table: "UykuTestleri",
                column: "HastaId",
                principalTable: "Hastalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
