using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class EditMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_pacients_DoctorId",
                table: "pacients",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_pacients_users_DoctorId",
                table: "pacients",
                column: "DoctorId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pacients_users_DoctorId",
                table: "pacients");

            migrationBuilder.DropIndex(
                name: "IX_pacients_DoctorId",
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
    }
}
