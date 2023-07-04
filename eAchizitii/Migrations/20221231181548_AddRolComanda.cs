using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class AddRolComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Angajati_comenzi",
                table: "Angajati_comenzi");

            migrationBuilder.AddColumn<int>(
                name: "RolComandaId",
                table: "Angajati_comenzi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Angajati_comenzi",
                table: "Angajati_comenzi",
                columns: new[] { "angajatId", "ComandaId", "RolComandaId" });

            migrationBuilder.CreateTable(
                name: "RoluriComenzi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoluriComenzi", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Angajati_comenzi_RolComandaId",
                table: "Angajati_comenzi",
                column: "RolComandaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Angajati_comenzi_RoluriComenzi_RolComandaId",
                table: "Angajati_comenzi",
                column: "RolComandaId",
                principalTable: "RoluriComenzi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Angajati_comenzi_RoluriComenzi_RolComandaId",
                table: "Angajati_comenzi");

            migrationBuilder.DropTable(
                name: "RoluriComenzi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Angajati_comenzi",
                table: "Angajati_comenzi");

            migrationBuilder.DropIndex(
                name: "IX_Angajati_comenzi_RolComandaId",
                table: "Angajati_comenzi");

            migrationBuilder.DropColumn(
                name: "RolComandaId",
                table: "Angajati_comenzi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Angajati_comenzi",
                table: "Angajati_comenzi",
                columns: new[] { "angajatId", "ComandaId" });
        }
    }
}
