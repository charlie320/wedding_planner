using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Migrations
{
    public partial class WeddingCoordinates2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Weddings",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Weddings",
                newName: "Latitude");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Weddings",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Weddings",
                newName: "latitude");
        }
    }
}
