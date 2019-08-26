using Microsoft.EntityFrameworkCore.Migrations;

namespace MaxiOrders.Back.Infrastructure.Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IdHeadQuarter",
                schema: "Masters",
                table: "Device",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Device_IdHeadQuarter",
                schema: "Masters",
                table: "Device",
                column: "IdHeadQuarter");

            migrationBuilder.AddForeignKey(
                name: "FK_Device_HeadQuarter",
                schema: "Masters",
                table: "Device",
                column: "IdHeadQuarter",
                principalSchema: "Companies",
                principalTable: "HeadQuarter",
                principalColumn: "IdHeadQuarter",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Device_HeadQuarter",
                schema: "Masters",
                table: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Device_IdHeadQuarter",
                schema: "Masters",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "IdHeadQuarter",
                schema: "Masters",
                table: "Device");
        }
    }
}
