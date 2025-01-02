using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class UpdateReviewAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDate_Activity_activityId",
                table: "ActivityDate");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Activity_ActivityId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "Review",
                newName: "ActivityDateId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ActivityId",
                table: "Review",
                newName: "IX_Review_ActivityDateId");

            migrationBuilder.RenameColumn(
                name: "activityId",
                table: "ActivityDate",
                newName: "ActivityId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ActivityDate",
                newName: "DateTime");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityDate_activityId",
                table: "ActivityDate",
                newName: "IX_ActivityDate_ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDate_Activity_ActivityId",
                table: "ActivityDate",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ActivityDate_ActivityDateId",
                table: "Review",
                column: "ActivityDateId",
                principalTable: "ActivityDate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDate_Activity_ActivityId",
                table: "ActivityDate");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_ActivityDate_ActivityDateId",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "ActivityDateId",
                table: "Review",
                newName: "ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ActivityDateId",
                table: "Review",
                newName: "IX_Review_ActivityId");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "ActivityDate",
                newName: "activityId");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "ActivityDate",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityDate_ActivityId",
                table: "ActivityDate",
                newName: "IX_ActivityDate_activityId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Reservation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDate_Activity_activityId",
                table: "ActivityDate",
                column: "activityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Activity_ActivityId",
                table: "Review",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
