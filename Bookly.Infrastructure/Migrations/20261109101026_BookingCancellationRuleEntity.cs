using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookingCancellationRuleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingCancellationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    DaysBeforeCheckIn = table.Column<int>(type: "int", nullable: false),
                    RefundPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCancellationRules", x => x.Id);
                    table.CheckConstraint("CK_BookingCancellationRules_DaysBeforeCheckIn", "[DaysBeforeCheckIn] >= 0");
                    table.CheckConstraint("CK_BookingCancellationRules_RefundPercentage", "[RefundPercentage] >= 0 AND [RefundPercentage] <= 100");
                    table.ForeignKey(
                        name: "FK_BookingCancellationRules_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingCancellationRules_BookingId",
                table: "BookingCancellationRules",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingCancellationRules");
        }
    }
}
