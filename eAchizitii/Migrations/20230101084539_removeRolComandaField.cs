using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class removeRolComandaField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rolAngajatComanda",
                table: "Angajati_comenzi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rolAngajatComanda",
                table: "Angajati_comenzi",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
