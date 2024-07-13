using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class initialcreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    BidListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AskQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ask = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Benchmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidListDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Commentary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Security = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Side = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.BidListId);
                });

            migrationBuilder.CreateTable(
                name: "CurvePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurveId = table.Column<int>(type: "int", nullable: false),
                    AsOfDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurvePoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoodysRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SandPRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FitchRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SqlStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SqlPart = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    TradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Benchmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Security = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourcelistId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Side = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "CurvePoints");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
