using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroPhotographyAPI.Migrations
{
    public partial class addedType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoThumbnail",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoType",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoThumbnail",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoType",
                table: "Photos");
        }
    }
}
