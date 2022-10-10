using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University_Information_System.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "AspNetUsers",
                newName: "PicturePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicturePath",
                table: "AspNetUsers",
                newName: "Picture");
        }
    }
}
