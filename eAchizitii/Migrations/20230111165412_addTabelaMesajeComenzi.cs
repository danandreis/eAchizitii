using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class addTabelaMesajeComenzi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MesajeComanda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComandaId = table.Column<int>(type: "int", nullable: true),
                    utilizatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MesajeComanda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MesajeComanda_AspNetUsers_utilizatorId",
                        column: x => x.utilizatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MesajeComanda_Comenzi_ComandaId",
                        column: x => x.ComandaId,
                        principalTable: "Comenzi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MesajeComanda_ComandaId",
                table: "MesajeComanda",
                column: "ComandaId");

            migrationBuilder.CreateIndex(
                name: "IX_MesajeComanda_utilizatorId",
                table: "MesajeComanda",
                column: "utilizatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MesajeComanda");
        }
    }
}
