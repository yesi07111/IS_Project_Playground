using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ChangeReviewRelationWithFacilityToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_AspNetUsers_EducatorIdId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Facility_FacilityId",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "Review",
                newName: "ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_FacilityId",
                table: "Review",
                newName: "IX_Review_ActivityId");

            migrationBuilder.RenameColumn(
                name: "EducatorIdId",
                table: "Activity",
                newName: "EducatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_EducatorIdId",
                table: "Activity",
                newName: "IX_Activity_EducatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_AspNetUsers_EducatorId",
                table: "Activity",
                column: "EducatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Activity_ActivityId",
                table: "Review",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_AspNetUsers_EducatorId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Activity_ActivityId",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "Review",
                newName: "FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ActivityId",
                table: "Review",
                newName: "IX_Review_FacilityId");

            migrationBuilder.RenameColumn(
                name: "EducatorId",
                table: "Activity",
                newName: "EducatorIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_EducatorId",
                table: "Activity",
                newName: "IX_Activity_EducatorIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_AspNetUsers_EducatorIdId",
                table: "Activity",
                column: "EducatorIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Facility_FacilityId",
                table: "Review",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
