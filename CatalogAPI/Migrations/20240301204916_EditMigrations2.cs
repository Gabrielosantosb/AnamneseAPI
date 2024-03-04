using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class EditMigrations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pacients_users_DoctorId",
                table: "pacients");

            migrationBuilder.DropIndex(
                name: "IX_pacients_DoctorId",
                table: "pacients");

            migrationBuilder.CreateTable(
                name: "PacientModelUserModel",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacientModelUserModel", x => new { x.DoctorId, x.PatientsId });
                    table.ForeignKey(
                        name: "FK_PacientModelUserModel_pacients_PatientsId",
                        column: x => x.PatientsId,
                        principalTable: "pacients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PacientModelUserModel_users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PacientModelUserModel_PatientsId",
                table: "PacientModelUserModel",
                column: "PatientsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PacientModelUserModel");

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
    }
}
