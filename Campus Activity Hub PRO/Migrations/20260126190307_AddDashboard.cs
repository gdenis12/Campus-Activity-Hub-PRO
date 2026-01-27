using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campus_Activity_Hub_PRO.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartsAt",
                table: "Events",
                newName: "EventDate");

            migrationBuilder.RenameIndex(
                name: "IX_Events_CategoryId_StartsAt",
                table: "Events",
                newName: "IX_Events_CategoryId_EventDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "Events",
                newName: "StartsAt");

            migrationBuilder.RenameIndex(
                name: "IX_Events_CategoryId_EventDate",
                table: "Events",
                newName: "IX_Events_CategoryId_StartsAt");
        }
    }
}
