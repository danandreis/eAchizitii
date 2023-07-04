using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class addCampNumeFisierBDTabelaFisier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numeFisier",
                table: "Fisiere",
                newName: "numeFisierDisc");

            migrationBuilder.AddColumn<string>(
                name: "numeFisierBazaDate",
                table: "Fisiere",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numeFisierBazaDate",
                table: "Fisiere");

            migrationBuilder.RenameColumn(
                name: "numeFisierDisc",
                table: "Fisiere",
                newName: "numeFisier");
        }
    }
}
