using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class adaugaTabelStatusComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Comenzi",
                newName: "StatusComandaId");

            migrationBuilder.CreateTable(
                name: "StatusComanda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusComanda", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comenzi_StatusComandaId",
                table: "Comenzi",
                column: "StatusComandaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_StatusComanda_StatusComandaId",
                table: "Comenzi",
                column: "StatusComandaId",
                principalTable: "StatusComanda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_StatusComanda_StatusComandaId",
                table: "Comenzi");

            migrationBuilder.DropTable(
                name: "StatusComanda");

            migrationBuilder.DropIndex(
                name: "IX_Comenzi_StatusComandaId",
                table: "Comenzi");

            migrationBuilder.RenameColumn(
                name: "StatusComandaId",
                table: "Comenzi",
                newName: "status");
        }
    }
}
