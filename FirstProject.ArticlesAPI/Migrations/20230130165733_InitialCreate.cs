using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstProject.ArticlesAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speciality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    RelatedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadData",
                columns: table => new
                {
                    LeadDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonTextHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadData", x => x.LeadDataId);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimePublished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCorporative = table.Column<bool>(type: "bit", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadDataId = table.Column<int>(type: "int", nullable: true),
                    EditorVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    RelatedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    RulesRemindEnabled = table.Column<bool>(type: "bit", nullable: false),
                    VotesEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlannedPublishTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Checked = table.Column<bool>(type: "bit", nullable: true),
                    HasPinnedComments = table.Column<bool>(type: "bit", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEditorial = table.Column<bool>(type: "bit", nullable: false),
                    ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Articles_LeadData_LeadDataId",
                        column: x => x.LeadDataId,
                        principalTable: "LeadData",
                        principalColumn: "LeadDataId");
                });

            migrationBuilder.CreateTable(
                name: "Flow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FlowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flow_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flow_Flow_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flow",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hub",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProfiled = table.Column<bool>(type: "bit", nullable: false),
                    RelatedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLoading = table.Column<bool>(type: "bit", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hub", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hub_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_LeadDataId",
                table: "Articles",
                column: "LeadDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Flow_ArticleId",
                table: "Flow",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Flow_FlowId",
                table: "Flow",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Hub_ArticleId",
                table: "Hub",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ArticleId",
                table: "Tag",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ArticleId",
                table: "Users",
                column: "ArticleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flow");

            migrationBuilder.DropTable(
                name: "Hub");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "LeadData");
        }
    }
}
