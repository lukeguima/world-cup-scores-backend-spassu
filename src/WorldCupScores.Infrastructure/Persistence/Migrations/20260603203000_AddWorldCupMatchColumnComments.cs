using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCupScores.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorldCupMatchColumnComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                COMMENT ON TABLE world_cup_matches IS 'World Cup match schedule, score and lifecycle data.';
                COMMENT ON COLUMN world_cup_matches."Id" IS 'Unique match identifier.';
                COMMENT ON COLUMN world_cup_matches."HomeTeam" IS 'Name of the home team.';
                COMMENT ON COLUMN world_cup_matches."AwayTeam" IS 'Name of the away team.';
                COMMENT ON COLUMN world_cup_matches."HomeScore" IS 'Optional home team score. Must be null while the match is scheduled.';
                COMMENT ON COLUMN world_cup_matches."AwayScore" IS 'Optional away team score. Must be null while the match is scheduled.';
                COMMENT ON COLUMN world_cup_matches."MatchDate" IS 'Date and time when the match is scheduled to happen, stored with time zone.';
                COMMENT ON COLUMN world_cup_matches."Stage" IS 'Competition stage. Values: 0=GroupStage, 1=RoundOf16, 2=QuarterFinal, 3=SemiFinal, 4=ThirdPlace, 5=Final.';
                COMMENT ON COLUMN world_cup_matches."Status" IS 'Match lifecycle status. Values: 0=Scheduled, 1=InProgress, 2=Finished, 3=Canceled.';
                COMMENT ON COLUMN world_cup_matches."Stadium" IS 'Optional stadium or venue where the match is played.';
                COMMENT ON COLUMN world_cup_matches."CreatedAt" IS 'UTC timestamp when the match record was created.';
                COMMENT ON COLUMN world_cup_matches."UpdatedAt" IS 'UTC timestamp of the latest match record update, null when never updated.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                COMMENT ON TABLE world_cup_matches IS NULL;
                COMMENT ON COLUMN world_cup_matches."Id" IS NULL;
                COMMENT ON COLUMN world_cup_matches."HomeTeam" IS NULL;
                COMMENT ON COLUMN world_cup_matches."AwayTeam" IS NULL;
                COMMENT ON COLUMN world_cup_matches."HomeScore" IS NULL;
                COMMENT ON COLUMN world_cup_matches."AwayScore" IS NULL;
                COMMENT ON COLUMN world_cup_matches."MatchDate" IS NULL;
                COMMENT ON COLUMN world_cup_matches."Stage" IS NULL;
                COMMENT ON COLUMN world_cup_matches."Status" IS NULL;
                COMMENT ON COLUMN world_cup_matches."Stadium" IS NULL;
                COMMENT ON COLUMN world_cup_matches."CreatedAt" IS NULL;
                COMMENT ON COLUMN world_cup_matches."UpdatedAt" IS NULL;
                """);
        }
    }
}
