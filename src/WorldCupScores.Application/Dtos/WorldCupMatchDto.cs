using WorldCupScores.Domain.Entities;
using WorldCupScores.Domain.Enums;

namespace WorldCupScores.Application.Dtos;

public sealed record WorldCupMatchDto(
    Guid Id,
    string HomeTeam,
    string AwayTeam,
    int? HomeScore,
    int? AwayScore,
    DateTime MatchDate,
    MatchStage Stage,
    MatchStatus Status,
    string? Stadium,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static WorldCupMatchDto FromEntity(WorldCupMatch match)
    {
        return new WorldCupMatchDto(
            match.Id,
            match.HomeTeam,
            match.AwayTeam,
            match.HomeScore,
            match.AwayScore,
            match.MatchDate,
            match.Stage,
            match.Status,
            match.Stadium,
            match.CreatedAt,
            match.UpdatedAt);
    }
}
