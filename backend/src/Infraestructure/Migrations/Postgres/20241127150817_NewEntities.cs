using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class NewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilitie",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    MaximumCapacity = table.Column<int>(type: "integer", nullable: false),
                    UsagePolicy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilitie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activitie",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CurrentParticipants = table.Column<int>(type: "integer", nullable: false),
                    EducatorIdId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    RecommendedAge = table.Column<int>(type: "integer", nullable: false),
                    ItsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    FacilityId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activitie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activitie_AspNetUsers_EducatorIdId",
                        column: x => x.EducatorIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activitie_Facilitie_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilitie",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ParentIdId = table.Column<string>(type: "text", nullable: true),
                    FacilityId = table.Column<string>(type: "text", nullable: false),
                    AdditionalComments = table.Column<string>(type: "text", nullable: false),
                    AmmountOfChildren = table.Column<int>(type: "integer", nullable: false),
                    ReservationState = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_AspNetUsers_ParentIdId",
                        column: x => x.ParentIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Facilitie_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilitie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    UseFrecuency = table.Column<float>(type: "real", nullable: false),
                    ResourceCondition = table.Column<string>(type: "text", nullable: false),
                    FacilityId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_Facilitie_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilitie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ParentIdId = table.Column<string>(type: "text", nullable: true),
                    FacilityId = table.Column<string>(type: "text", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_ParentIdId",
                        column: x => x.ParentIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_Facilitie_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilitie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activitie_EducatorIdId",
                table: "Activitie",
                column: "EducatorIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Activitie_FacilityId",
                table: "Activitie",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_FacilityId",
                table: "Reservation",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ParentIdId",
                table: "Reservation",
                column: "ParentIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_FacilityId",
                table: "Resource",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_FacilityId",
                table: "Review",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ParentIdId",
                table: "Review",
                column: "ParentIdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activitie");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Facilitie");
        }
    }
}
