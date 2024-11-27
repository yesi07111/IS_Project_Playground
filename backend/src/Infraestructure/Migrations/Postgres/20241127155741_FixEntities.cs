using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class FixEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Resource");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Review",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Review",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Review",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Review",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Resource",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Resource",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Resource",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Resource",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reservation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Reservation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reservation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Reservation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Facilitie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Facilitie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Facilitie",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Facilitie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Activitie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Activitie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Activitie",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Activitie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Facilitie");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Facilitie");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Facilitie");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Facilitie");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Activitie");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Activitie");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Activitie");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Activitie");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Resource",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
