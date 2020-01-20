using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityDbContnext.Migrations
{
    public partial class updatedatabse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ali1688ProductLink",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Sales = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ali1688ProductLink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ali1688ProductLinkImg",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ali1688ProductLinkId = table.Column<long>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ali1688ProductLinkImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ali1688ProductLinkImg_Ali1688ProductLink_Ali1688ProductLinkId",
                        column: x => x.Ali1688ProductLinkId,
                        principalTable: "Ali1688ProductLink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ali1688ProductLinkImg_Ali1688ProductLinkId",
                table: "Ali1688ProductLinkImg",
                column: "Ali1688ProductLinkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ali1688ProductLinkImg");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Ali1688ProductLink");
        }
    }
}
