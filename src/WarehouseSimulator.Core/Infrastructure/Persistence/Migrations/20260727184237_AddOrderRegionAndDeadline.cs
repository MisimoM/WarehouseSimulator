using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseSimulator.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRegionAndDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimulatedCreatedAt",
                table: "Orders",
                newName: "DeliveryDeadline");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryDeadline",
                table: "Orders",
                newName: "SimulatedCreatedAt");
        }
    }
}
