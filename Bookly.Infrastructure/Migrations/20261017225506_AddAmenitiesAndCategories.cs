using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAmenitiesAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "RoomTypes",
                newName: "NameEn");

            migrationBuilder.CreateTable(
                name: "AmenityCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Amenity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenity_AmenityCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AmenityCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_CategoryId",
                table: "Amenity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_NameAr",
                table: "Amenity",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_NameEn",
                table: "Amenity",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmenityCategory_NameAr",
                table: "AmenityCategory",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmenityCategory_NameEn",
                table: "AmenityCategory",
                column: "NameEn",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenity");

            migrationBuilder.DropTable(
                name: "AmenityCategory");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "RoomTypes",
                newName: "Name");
        }
    }
}
