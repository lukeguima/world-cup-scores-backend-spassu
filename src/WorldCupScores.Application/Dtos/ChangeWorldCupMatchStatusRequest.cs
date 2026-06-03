using WorldCupScores.Domain.Enums;

namespace WorldCupScores.Application.Dtos;

public sealed record ChangeWorldCupMatchStatusRequest(MatchStatus Status);
