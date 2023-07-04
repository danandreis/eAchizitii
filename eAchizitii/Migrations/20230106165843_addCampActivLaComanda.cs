using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class addCampActivLaComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_AdreseLivrare_AdresaLivrareId",
                table: "Comenzi");

            migrationBuilder.AlterColumn<int>(
                name: "AdresaLivrareId",
                table: "Comenzi",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Activ",
                table: "Comenzi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_AdreseLivrare_AdresaLivrareId",
                table: "Comenzi",
                column: "AdresaLivrareId",
                principalTable: "AdreseLivrare",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_AdreseLivrare_AdresaLivrareId",
                table: "Comenzi");

            migrationBuilder.DropColumn(
                name: "Activ",
                table: "Comenzi");

            migrationBuilder.AlterColumn<int>(
                name: "AdresaLivrareId",
                table: "Comenzi",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_AdreseLivrare_AdresaLivrareId",
                table: "Comenzi",
                column: "AdresaLivrareId",
                principalTable: "AdreseLivrare",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
