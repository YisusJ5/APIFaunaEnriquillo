using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIFaunaEnriquillo.InfraestructureLayer.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Habitats",
                columns: table => new
                {
                    IdHabitat = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreComun = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NombreCientifico = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Clima = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UbicacionGeograficaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKHabitat", x => x.IdHabitat);
                });

            migrationBuilder.CreateTable(
                name: "Animales",
                columns: table => new
                {
                    IdAnimal = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreComun = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NombreCientifico = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Dieta = table.Column<int>(type: "int", nullable: false),
                    EstadoDeConservacion = table.Column<int>(type: "int", nullable: false),
                    FormaDeReproduccion = table.Column<int>(type: "int", nullable: false),
                    TipoDesarrolloEmbrionario = table.Column<int>(type: "int", nullable: false),
                    EstatusBiogeográfico = table.Column<int>(type: "int", nullable: false),
                    Filo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Clase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Familia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Especie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubEspecie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    DistribucionGeograficaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HabitatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKAnimal", x => x.IdAnimal);
                    table.ForeignKey(
                        name: "FK_Animales_Habitats_HabitatId",
                        column: x => x.HabitatId,
                        principalTable: "Habitats",
                        principalColumn: "IdHabitat",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Plantas",
                columns: table => new
                {
                    IdPlanta = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreComun = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NombreCientifico = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EstadoDeConservacion = table.Column<int>(type: "int", nullable: false),
                    EstatusBiogeografico = table.Column<int>(type: "int", nullable: false),
                    Filo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Clase = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Orden = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Familia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Especie = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SubEspecie = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    DistribucionGeograficaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HabitatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKPlanta", x => x.IdPlanta);
                    table.ForeignKey(
                        name: "FK_Plantas_Habitats_HabitatId",
                        column: x => x.HabitatId,
                        principalTable: "Habitats",
                        principalColumn: "IdHabitat",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animales_HabitatId",
                table: "Animales",
                column: "HabitatId");

            migrationBuilder.CreateIndex(
                name: "IX_Plantas_HabitatId",
                table: "Plantas",
                column: "HabitatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animales");

            migrationBuilder.DropTable(
                name: "Plantas");

            migrationBuilder.DropTable(
                name: "Habitats");
        }
    }
}
