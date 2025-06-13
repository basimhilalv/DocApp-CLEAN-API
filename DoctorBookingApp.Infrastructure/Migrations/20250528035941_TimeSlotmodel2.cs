using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class TimeSlotmodel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_Doctors_DoctorId",
                table: "TimeSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSlot",
                table: "TimeSlot");

            migrationBuilder.RenameTable(
                name: "TimeSlot",
                newName: "TimeSlots");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSlot_DoctorId",
                table: "TimeSlots",
                newName: "IX_TimeSlots_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSlots",
                table: "TimeSlots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlots_Doctors_DoctorId",
                table: "TimeSlots",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_Doctors_DoctorId",
                table: "TimeSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSlots",
                table: "TimeSlots");

            migrationBuilder.RenameTable(
                name: "TimeSlots",
                newName: "TimeSlot");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSlots_DoctorId",
                table: "TimeSlot",
                newName: "IX_TimeSlot_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSlot",
                table: "TimeSlot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_Doctors_DoctorId",
                table: "TimeSlot",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
