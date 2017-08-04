using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.Web.Domain.Migrations
{
    public partial class NewsPublish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "News",
                newName: "Created");

            migrationBuilder.AddColumn<bool>(
                name: "HasEverBeenPublished",
                table: "News",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "News",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Updated",
                table: "News",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.Sql("UPDATE News SET Updated = Created");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "News",
                newName: "Date");

            migrationBuilder.DropColumn(
                name: "HasEverBeenPublished",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "News");
        }
    }
}
