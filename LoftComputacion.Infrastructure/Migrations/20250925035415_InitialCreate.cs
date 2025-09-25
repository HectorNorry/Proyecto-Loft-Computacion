using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoftComputacion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Componentes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroDeSerie = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetodosDePago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosDePago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesDeServicio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FallaDeclaradaPorCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNotificacionRetiro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrecioPresupuestado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecioFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    MetodoDePagoId = table.Column<int>(type: "int", nullable: true),
                    EquipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesDeServicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesDeServicio_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesDeServicio_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesDeServicio_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesDeServicio_MetodosDePago_MetodoDePagoId",
                        column: x => x.MetodoDePagoId,
                        principalTable: "MetodosDePago",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdenDeServicioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotos_OrdenesDeServicio_OrdenDeServicioId",
                        column: x => x.OrdenDeServicioId,
                        principalTable: "OrdenesDeServicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialOrdenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DescripcionDelCambio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdenDeServicioId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialOrdenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialOrdenes_OrdenesDeServicio_OrdenDeServicioId",
                        column: x => x.OrdenDeServicioId,
                        principalTable: "OrdenesDeServicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialOrdenes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_OrdenDeServicioId",
                table: "Fotos",
                column: "OrdenDeServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialOrdenes_OrdenDeServicioId",
                table: "HistorialOrdenes",
                column: "OrdenDeServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialOrdenes_UsuarioId",
                table: "HistorialOrdenes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeServicio_ClienteId",
                table: "OrdenesDeServicio",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeServicio_EquipoId",
                table: "OrdenesDeServicio",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeServicio_EstadoId",
                table: "OrdenesDeServicio",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeServicio_MetodoDePagoId",
                table: "OrdenesDeServicio",
                column: "MetodoDePagoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropTable(
                name: "HistorialOrdenes");

            migrationBuilder.DropTable(
                name: "OrdenesDeServicio");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "MetodosDePago");
        }
    }
}
