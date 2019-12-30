using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroPhotographyAPI.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhotoName = table.Column<string>(nullable: true),
                    PhotoLocation = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PhotoPath = table.Column<string>(nullable: true),
                    MainCamera = table.Column<string>(nullable: true),
                    MainTelescope = table.Column<string>(nullable: true),
                    Mount = table.Column<string>(nullable: true),
                    GuideScope = table.Column<string>(nullable: true),
                    GuideCamera = table.Column<string>(nullable: true),
                    Filters = table.Column<string>(nullable: true),
                    Other = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
