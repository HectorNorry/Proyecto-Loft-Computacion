using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftComputacion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDniToCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DNI",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_DNI",
                table: "Clientes",
                column: "DNI",
                unique: true,
                filter: "[DNI] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clientes_DNI",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "DNI",
                table: "Clientes");
        }
    }
}
