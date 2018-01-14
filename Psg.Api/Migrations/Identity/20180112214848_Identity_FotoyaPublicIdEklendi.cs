using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Psg.Api.Migrations.Identity
{
    public partial class Identity_FotoyaPublicIdEklendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                schema: "Polisomnografi",
                table: "Fotograflar",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                schema: "Polisomnografi",
                table: "Fotograflar");
        }
    }
}
