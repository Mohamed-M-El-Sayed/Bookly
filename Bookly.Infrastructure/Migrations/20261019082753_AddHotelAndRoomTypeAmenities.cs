using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelAndRoomTypeAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenity_AmenityCategory_CategoryId",
                table: "Amenity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmenityCategory",
                table: "AmenityCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amenity",
                table: "Amenity");

            migrationBuilder.RenameTable(
                name: "AmenityCategory",
                newName: "AmenityCategories");

            migrationBuilder.RenameTable(
                name: "Amenity",
                newName: "Amenities");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityCategory_NameEn",
                table: "AmenityCategories",
                newName: "IX_AmenityCategories_NameEn");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityCategory_NameAr",
                table: "AmenityCategories",
                newName: "IX_AmenityCategories_NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Amenity_NameEn",
                table: "Amenities",
                newName: "IX_Amenities_NameEn");

            migrationBuilder.RenameIndex(
                name: "IX_Amenity_NameAr",
                table: "Amenities",
                newName: "IX_Amenities_NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Amenity_CategoryId",
                table: "Amenities",
                newName: "IX_Amenities_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmenityCategories",
                table: "AmenityCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amenities",
                table: "Amenities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HotelAmenity",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAmenity", x => new { x.HotelId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_HotelAmenity_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HotelAmenity_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypeAmenity",
                columns: table => new
                {
                    AmenityId = table.Column<int>(type: "int", nullable: false),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypeAmenity", x => new { x.RoomTypeId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_RoomTypeAmenity_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomTypeAmenity_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenity_AmenityId",
                table: "HotelAmenity",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeAmenity_AmenityId",
                table: "RoomTypeAmenity",
                column: "AmenityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenities_AmenityCategories_CategoryId",
                table: "Amenities",
                column: "CategoryId",
                principalTable: "AmenityCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenities_AmenityCategories_CategoryId",
                table: "Amenities");

            migrationBuilder.DropTable(
                name: "HotelAmenity");

            migrationBuilder.DropTable(
                name: "RoomTypeAmenity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmenityCategories",
                table: "AmenityCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amenities",
                table: "Amenities");

            migrationBuilder.RenameTable(
                name: "AmenityCategories",
                newName: "AmenityCategory");

            migrationBuilder.RenameTable(
                name: "Amenities",
                newName: "Amenity");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityCategories_NameEn",
                table: "AmenityCategory",
                newName: "IX_AmenityCategory_NameEn");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityCategories_NameAr",
                table: "AmenityCategory",
                newName: "IX_AmenityCategory_NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Amenities_NameEn",
                table: "Amenity",
                newName: "IX_Amenity_NameEn");

            migrationBuilder.RenameIndex(
                name: "IX_Amenities_NameAr",
                table: "Amenity",
                newName: "IX_Amenity_NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Amenities_CategoryId",
                table: "Amenity",
                newName: "IX_Amenity_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmenityCategory",
                table: "AmenityCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amenity",
                table: "Amenity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenity_AmenityCategory_CategoryId",
                table: "Amenity",
                column: "CategoryId",
                principalTable: "AmenityCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
