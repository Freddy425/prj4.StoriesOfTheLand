using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoriesOfTheLand.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faq",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SpecimenID = table.Column<int>(type: "INTEGER", nullable: true),
                    Details = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.FeedbackID);
                });

            migrationBuilder.CreateTable(
                name: "FR_Resource",
                columns: table => new
                {
                    ResourceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FR_ResourceTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FR_ResourceDescription = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Resource", x => x.ResourceID);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    ResourceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResourceTitle = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ResourceDescription = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ResourceURL = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ResourceImage = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.ResourceID);
                });

            migrationBuilder.CreateTable(
                name: "Sponsor",
                columns: table => new
                {
                    SponsorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SponsorName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SponsorURL = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SponsorImagePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsor", x => x.SponsorID);
                });

            migrationBuilder.CreateTable(
                name: "UserImage",
                columns: table => new
                {
                    UserImageiD = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IP = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    FileSize = table.Column<int>(type: "INTEGER", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    status = table.Column<bool>(type: "INTEGER", nullable: false),
                    MediaPath = table.Column<string>(type: "TEXT", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImage", x => x.UserImageiD);
                });

            migrationBuilder.CreateTable(
                name: "Specimen",
                columns: table => new
                {
                    SpecimenID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LatinName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SpecimenDescription = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreeName = table.Column<string>(type: "TEXT", maxLength: 90, nullable: true),
                    CulturalSignificance = table.Column<string>(type: "TEXT", maxLength: 3500, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    FR_ResourceResourceID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specimen", x => x.SpecimenID);
                    table.ForeignKey(
                        name: "FK_Specimen_FR_Resource_FR_ResourceResourceID",
                        column: x => x.FR_ResourceResourceID,
                        principalTable: "FR_Resource",
                        principalColumn: "ResourceID");
                });

            migrationBuilder.CreateTable(
                name: "FR_Specimen",
                columns: table => new
                {
                    SpecimenID = table.Column<int>(type: "INTEGER", nullable: false),
                    FR_SpecimenDescription = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false),
                    FR_EnglishName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FR_CulturalSignificance = table.Column<string>(type: "TEXT", maxLength: 3500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Specimen", x => x.SpecimenID);
                    table.ForeignKey(
                        name: "FK_FR_Specimen_Specimen_SpecimenID",
                        column: x => x.SpecimenID,
                        principalTable: "Specimen",
                        principalColumn: "SpecimenID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpecimenID = table.Column<int>(type: "INTEGER", nullable: false),
                    MediaType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MediaPath = table.Column<string>(type: "TEXT", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Media_Specimen_SpecimenID",
                        column: x => x.SpecimenID,
                        principalTable: "Specimen",
                        principalColumn: "SpecimenID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Media_SpecimenID",
                table: "Media",
                column: "SpecimenID");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_FR_ResourceResourceID",
                table: "Specimen",
                column: "FR_ResourceResourceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faq");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "FR_Specimen");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Sponsor");

            migrationBuilder.DropTable(
                name: "UserImage");

            migrationBuilder.DropTable(
                name: "Specimen");

            migrationBuilder.DropTable(
                name: "FR_Resource");
        }
    }
}
