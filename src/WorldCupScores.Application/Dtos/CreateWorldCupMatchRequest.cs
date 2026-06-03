using WorldCupScores.Domain.Enums;

namespace WorldCupScores.Application.Dtos;

public sealed record CreateWorldCupMatchRequest(
    string HomeTeam,
    string AwayTeam,
    DateTime MatchDate,
    MatchStage Stage,
    string? Stadium);
