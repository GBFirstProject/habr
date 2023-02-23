using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstProject.ArticlesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<string>(
                name: "AuthorNickName",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false);            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Articles");
        }
    }
}
