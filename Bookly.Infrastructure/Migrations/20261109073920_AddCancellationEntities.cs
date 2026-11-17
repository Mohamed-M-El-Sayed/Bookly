using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCancellationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingCancellations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCancellations", x => x.Id);
                    table.CheckConstraint("CK_BookingCancellations_CancellationFee", "[CancellationFee] >= 0");
                    table.CheckConstraint("CK_BookingCancellations_RefundAmount", "[RefundAmount] >= 0");
                    table.ForeignKey(
                        name: "FK_BookingCancellations_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CancellationPolicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationPolicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancellationPolicy_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CancellationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancellationPolicyId = table.Column<int>(type: "int", nullable: false),
                    DaysBeforeCheckIn = table.Column<int>(type: "int", nullable: false),
                    RefundPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationRules", x => x.Id);
                    table.CheckConstraint("CK_CancellationRules_DaysBeforeCheckIn", "[DaysBeforeCheckIn] >= 0");
                    table.CheckConstraint("CK_CancellationRules_RefundPercentage", "[RefundPercentage] >= 0 AND [RefundPercentage] <= 100");
                    table.ForeignKey(
                        name: "FK_CancellationRules_CancellationPolicy_CancellationPolicyId",
                        column: x => x.CancellationPolicyId,
                        principalTable: "CancellationPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingCancellations_BookingId",
                table: "BookingCancellations",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CancellationPolicy_HotelId",
                table: "CancellationPolicy",
                column: "HotelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRules_CancellationPolicyId_DaysBeforeCheckIn",
                table: "CancellationRules",
                columns: new[] { "CancellationPolicyId", "DaysBeforeCheckIn" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingCancellations");

            migrationBuilder.DropTable(
                name: "CancellationRules");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "CancellationPolicy");
        }
    }
}
