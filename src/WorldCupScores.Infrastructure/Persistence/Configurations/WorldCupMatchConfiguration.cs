using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCupScores.Domain.Entities;

namespace WorldCupScores.Infrastructure.Persistence.Configurations;

public sealed class WorldCupMatchConfiguration : IEntityTypeConfiguration<WorldCupMatch>
{
    public void Configure(EntityTypeBuilder<WorldCupMatch> builder)
    {
        builder.ToTable("world_cup_matches", table =>
            table.HasComment("World Cup match schedule, score and lifecycle data."));

        builder.HasKey(match => match.Id);

        builder.Property(match => match.Id)
            .HasComment("Unique match identifier.");

        builder.Property(match => match.HomeTeam)
            .HasMaxLength(120)
            .IsRequired()
            .HasComment("Name of the home team.");

        builder.Property(match => match.AwayTeam)
            .HasMaxLength(120)
            .IsRequired()
            .HasComment("Name of the away team.");

        builder.Property(match => match.MatchDate)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasComment("Date and time when the match is scheduled to happen, stored with time zone.");

        builder.Property(match => match.Stage)
            .HasConversion<int>()
            .IsRequired()
            .HasComment("Competition stage. Values: 0=GroupStage, 1=RoundOf16, 2=QuarterFinal, 3=SemiFinal, 4=ThirdPlace, 5=Final.");

        builder.Property(match => match.Status)
            .HasConversion<int>()
            .IsRequired()
            .HasComment("Match lifecycle status. Values: 0=Scheduled, 1=InProgress, 2=Finished, 3=Canceled.");

        builder.Property(match => match.Stadium)
            .HasMaxLength(160)
            .HasComment("Optional stadium or venue where the match is played.");

        builder.Property(match => match.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasComment("UTC timestamp when the match record was created.");

        builder.Property(match => match.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .HasComment("UTC timestamp of the latest match record update, null when never updated.");

        builder.Property(match => match.HomeScore)
            .HasComment("Optional home team score. Must be null while the match is scheduled.");

        builder.Property(match => match.AwayScore)
            .HasComment("Optional away team score. Must be null while the match is scheduled.");
    }
}
