using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyWayRide.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removedvehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Vehicles_VehicleID",
                table: "RideRequests");

            migrationBuilder.DropIndex(
                name: "IX_RideRequests_VehicleID",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "RideRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VehicleID",
                table: "RideRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_VehicleID",
                table: "RideRequests",
                column: "VehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Vehicles_VehicleID",
                table: "RideRequests",
                column: "VehicleID",
                principalTable: "Vehicles",
                principalColumn: "ID");
        }
    }
}
