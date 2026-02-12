using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftComputacion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTokenRecuperacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenRecuperacion",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenRecuperacionExpiracion",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenRecuperacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TokenRecuperacionExpiracion",
                table: "Usuarios");
        }
    }
}
