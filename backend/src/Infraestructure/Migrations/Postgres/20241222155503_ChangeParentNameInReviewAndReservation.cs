using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ChangeParentNameInReviewAndReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_AspNetUsers_ParentIdId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_ParentIdId",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "ParentIdId",
                table: "Review",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ParentIdId",
                table: "Review",
                newName: "IX_Review_ParentId");

            migrationBuilder.RenameColumn(
                name: "ParentIdId",
                table: "Reservation",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ParentIdId",
                table: "Reservation",
                newName: "IX_Reservation_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_AspNetUsers_ParentId",
                table: "Reservation",
                column: "ParentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_ParentId",
                table: "Review",
                column: "ParentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_AspNetUsers_ParentId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_ParentId",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Review",
                newName: "ParentIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ParentId",
                table: "Review",
                newName: "IX_Review_ParentIdId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Reservation",
                newName: "ParentIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ParentId",
                table: "Reservation",
                newName: "IX_Reservation_ParentIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_AspNetUsers_ParentIdId",
                table: "Reservation",
                column: "ParentIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_ParentIdId",
                table: "Review",
                column: "ParentIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
