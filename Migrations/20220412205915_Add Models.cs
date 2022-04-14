using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagment.Migrations
{
    public partial class AddModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patron_LibraryCards_LibraryCardId",
                table: "Patron");

            migrationBuilder.DropIndex(
                name: "IX_Patron_LibraryCardId",
                table: "Patron");

            migrationBuilder.DropColumn(
                name: "LibraryCardId",
                table: "Patron");

            migrationBuilder.AddColumn<int>(
                name: "PatronId",
                table: "LibraryCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryCards_PatronId",
                table: "LibraryCards",
                column: "PatronId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryCards_Patron_PatronId",
                table: "LibraryCards",
                column: "PatronId",
                principalTable: "Patron",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryCards_Patron_PatronId",
                table: "LibraryCards");

            migrationBuilder.DropIndex(
                name: "IX_LibraryCards_PatronId",
                table: "LibraryCards");

            migrationBuilder.DropColumn(
                name: "PatronId",
                table: "LibraryCards");

            migrationBuilder.AddColumn<int>(
                name: "LibraryCardId",
                table: "Patron",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Patron_LibraryCardId",
                table: "Patron",
                column: "LibraryCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patron_LibraryCards_LibraryCardId",
                table: "Patron",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
