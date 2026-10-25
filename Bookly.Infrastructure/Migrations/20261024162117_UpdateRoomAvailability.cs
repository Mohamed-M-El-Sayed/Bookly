using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_RoomAvailability_Balance",
                table: "RoomAvailabilities");

            migrationBuilder.DropColumn(
                name: "AvailableRooms",
                table: "RoomAvailabilities");

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId1",
                table: "RoomAvailabilities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailabilities_RoomTypeId1",
                table: "RoomAvailabilities",
                column: "RoomTypeId1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RoomAvailability_Balance",
                table: "RoomAvailabilities",
                sql: "[TotalRooms] >= 0 AND [ReservedRooms] >= 0 AND [ReservedRooms] <= [TotalRooms]");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilities_RoomTypes_RoomTypeId1",
                table: "RoomAvailabilities",
                column: "RoomTypeId1",
                principalTable: "RoomTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilities_RoomTypes_RoomTypeId1",
                table: "RoomAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_RoomAvailabilities_RoomTypeId1",
                table: "RoomAvailabilities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RoomAvailability_Balance",
                table: "RoomAvailabilities");

            migrationBuilder.DropColumn(
                name: "RoomTypeId1",
                table: "RoomAvailabilities");

            migrationBuilder.AddColumn<int>(
                name: "AvailableRooms",
                table: "RoomAvailabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_RoomAvailability_Balance",
                table: "RoomAvailabilities",
                sql: "[TotalRooms] = [AvailableRooms] + [ReservedRooms]");
        }
    }
}
