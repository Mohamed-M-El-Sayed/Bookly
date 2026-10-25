using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRoomAvailabilityRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilities_RoomTypes_RoomTypeId1",
                table: "RoomAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_RoomAvailabilities_RoomTypeId1",
                table: "RoomAvailabilities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RoomAvailabilities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoomAvailabilities");

            migrationBuilder.DropColumn(
                name: "RoomTypeId1",
                table: "RoomAvailabilities");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rooms");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RoomAvailabilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoomAvailabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId1",
                table: "RoomAvailabilities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailabilities_RoomTypeId1",
                table: "RoomAvailabilities",
                column: "RoomTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilities_RoomTypes_RoomTypeId1",
                table: "RoomAvailabilities",
                column: "RoomTypeId1",
                principalTable: "RoomTypes",
                principalColumn: "Id");
        }
    }
}
