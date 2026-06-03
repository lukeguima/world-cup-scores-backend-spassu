using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCupScores.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateWorldCupMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "world_cup_matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HomeTeam = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    AwayTeam = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    HomeScore = table.Column<int>(type: "integer", nullable: true),
                    AwayScore = table.Column<int>(type: "integer", nullable: true),
                    MatchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Stage = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Stadium = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_world_cup_matches", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "world_cup_matches");
        }
    }
}
