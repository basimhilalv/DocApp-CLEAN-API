using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class appointmentfeeamount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FeeAmount",
                table: "Appointments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeTransactionId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeeAmount",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StripeTransactionId",
                table: "Appointments");
        }
    }
}
