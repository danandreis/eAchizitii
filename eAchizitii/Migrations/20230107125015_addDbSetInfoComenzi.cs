using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAchizitii.Migrations
{
    /// <inheritdoc />
    public partial class addDbSetInfoComenzi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoComanda_AspNetUsers_utilizatorId",
                table: "InfoComanda");

            migrationBuilder.DropForeignKey(
                name: "FK_InfoComanda_Comenzi_ComandaId",
                table: "InfoComanda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InfoComanda",
                table: "InfoComanda");

            migrationBuilder.RenameTable(
                name: "InfoComanda",
                newName: "InfoComenzi");

            migrationBuilder.RenameIndex(
                name: "IX_InfoComanda_utilizatorId",
                table: "InfoComenzi",
                newName: "IX_InfoComenzi_utilizatorId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoComanda_ComandaId",
                table: "InfoComenzi",
                newName: "IX_InfoComenzi_ComandaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InfoComenzi",
                table: "InfoComenzi",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoComenzi_AspNetUsers_utilizatorId",
                table: "InfoComenzi",
                column: "utilizatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InfoComenzi_Comenzi_ComandaId",
                table: "InfoComenzi",
                column: "ComandaId",
                principalTable: "Comenzi",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoComenzi_AspNetUsers_utilizatorId",
                table: "InfoComenzi");

            migrationBuilder.DropForeignKey(
                name: "FK_InfoComenzi_Comenzi_ComandaId",
                table: "InfoComenzi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InfoComenzi",
                table: "InfoComenzi");

            migrationBuilder.RenameTable(
                name: "InfoComenzi",
                newName: "InfoComanda");

            migrationBuilder.RenameIndex(
                name: "IX_InfoComenzi_utilizatorId",
                table: "InfoComanda",
                newName: "IX_InfoComanda_utilizatorId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoComenzi_ComandaId",
                table: "InfoComanda",
                newName: "IX_InfoComanda_ComandaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InfoComanda",
                table: "InfoComanda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoComanda_AspNetUsers_utilizatorId",
                table: "InfoComanda",
                column: "utilizatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InfoComanda_Comenzi_ComandaId",
                table: "InfoComanda",
                column: "ComandaId",
                principalTable: "Comenzi",
                principalColumn: "Id");
        }
    }
}
