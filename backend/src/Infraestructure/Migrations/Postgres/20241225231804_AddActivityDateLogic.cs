using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddActivityDateLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActivityDateId",
                table: "Reservation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ActivityDate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    activityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReservedPlaces = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityDate_Activity_activityId",
                        column: x => x.activityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ActivityDateId",
                table: "Reservation",
                column: "ActivityDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDate_activityId",
                table: "ActivityDate",
                column: "activityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_ActivityDate_ActivityDateId",
                table: "Reservation",
                column: "ActivityDateId",
                principalTable: "ActivityDate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ActivityDate_ActivityDateId",
                table: "Reservation");

            migrationBuilder.DropTable(
                name: "ActivityDate");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ActivityDateId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ActivityDateId",
                table: "Reservation");
        }
    }
}
