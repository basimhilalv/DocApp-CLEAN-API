using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class doctorverify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Doctors");
        }
    }
}
