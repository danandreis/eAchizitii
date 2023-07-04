using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class adaugaTabelStatusComanda2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_StatusComanda_StatusComandaId",
                table: "Comenzi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusComanda",
                table: "StatusComanda");

            migrationBuilder.RenameTable(
                name: "StatusComanda",
                newName: "StatusuriComanda");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusuriComanda",
                table: "StatusuriComanda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_StatusuriComanda_StatusComandaId",
                table: "Comenzi",
                column: "StatusComandaId",
                principalTable: "StatusuriComanda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_StatusuriComanda_StatusComandaId",
                table: "Comenzi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusuriComanda",
                table: "StatusuriComanda");

            migrationBuilder.RenameTable(
                name: "StatusuriComanda",
                newName: "StatusComanda");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusComanda",
                table: "StatusComanda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_StatusComanda_StatusComandaId",
                table: "Comenzi",
                column: "StatusComandaId",
                principalTable: "StatusComanda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
