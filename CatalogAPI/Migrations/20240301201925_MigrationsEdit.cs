using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pacients_users_doctorId",
                table: "pacients");

            migrationBuilder.DropIndex(
                name: "IX_pacients_doctorId",
                table: "pacients");

            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "pacients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pacients_UserModelId",
                table: "pacients",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_pacients_users_UserModelId",
                table: "pacients",
                column: "UserModelId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pacients_users_UserModelId",
                table: "pacients");

            migrationBuilder.DropIndex(
                name: "IX_pacients_UserModelId",
                table: "pacients");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "pacients");

            migrationBuilder.CreateIndex(
                name: "IX_pacients_doctorId",
                table: "pacients",
                column: "doctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_pacients_users_doctorId",
                table: "pacients",
                column: "doctorId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
