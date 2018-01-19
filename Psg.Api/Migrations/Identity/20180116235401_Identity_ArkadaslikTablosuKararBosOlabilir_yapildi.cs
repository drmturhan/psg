using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.Migrations.Identity
{
    public partial class Identity_ArkadaslikTablosuKararBosOlabilir_yapildi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Karar",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Karar",
                schema: "Polisomnografi",
                table: "ArkadaslikTeklifleri",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
