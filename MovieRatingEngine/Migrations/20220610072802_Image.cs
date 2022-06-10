using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieRatingEngine.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByteArray",
                table: "Movies",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageByteArray",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Movies");
        }
    }
}
