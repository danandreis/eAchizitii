using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class addUtilizatorIDLaInfoComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "utilizatorId",
                table: "InfoComanda",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_InfoComanda_utilizatorId",
                table: "InfoComanda",
                column: "utilizatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoComanda_AspNetUsers_utilizatorId",
                table: "InfoComanda",
                column: "utilizatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoComanda_AspNetUsers_utilizatorId",
                table: "InfoComanda");

            migrationBuilder.DropIndex(
                name: "IX_InfoComanda_utilizatorId",
                table: "InfoComanda");

            migrationBuilder.DropColumn(
                name: "utilizatorId",
                table: "InfoComanda");
        }
    }
}
