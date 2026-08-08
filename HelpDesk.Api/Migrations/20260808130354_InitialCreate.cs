using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RaisedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedDate", "Description", "Priority", "RaisedBy", "Status", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Getting error 503 when logging into Outlook on web.", "High", "John Doe", "Open", "Cannot access corporate email" },
                    { 2, new DateTime(2026, 8, 2, 11, 30, 0, 0, DateTimeKind.Utc), "The VPN connection drops every 15 minutes on Windows 11.", "Medium", "Jane Smith", "In Progress", "VPN connection drops" },
                    { 3, new DateTime(2026, 8, 3, 14, 15, 0, 0, DateTimeKind.Utc), "Requesting a secondary 27-inch monitor for development workspace.", "Low", "Alice Johnson", "Closed", "New monitor request" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
