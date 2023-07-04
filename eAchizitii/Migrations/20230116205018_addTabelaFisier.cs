using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class addTabelaFisier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fisiere",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeFisier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    caleFisier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InfoComandaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fisiere", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fisiere_InfoComenzi_InfoComandaId",
                        column: x => x.InfoComandaId,
                        principalTable: "InfoComenzi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fisiere_InfoComandaId",
                table: "Fisiere",
                column: "InfoComandaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fisiere");
        }
    }
}
