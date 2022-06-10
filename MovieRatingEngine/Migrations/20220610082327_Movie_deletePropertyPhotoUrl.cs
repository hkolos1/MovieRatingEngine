using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieRatingEngine.Migrations
{
    public partial class Movie_deletePropertyPhotoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
