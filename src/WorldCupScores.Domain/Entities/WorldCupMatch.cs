using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Exceptions;

namespace WorldCupScores.Domain.Entities;

public sealed class WorldCupMatch
{
    private WorldCupMatch()
    {
        HomeTeam = string.Empty;
        AwayTeam = string.Empty;
    }

    private WorldCupMatch(
        Guid id,
        string homeTeam,
        string awayTeam,
        DateTime matchDate,
        MatchStage stage,
        string? stadium)
    {
        Id = id;
        HomeTeam = NormalizeRequired(homeTeam, nameof(HomeTeam));
        AwayTeam = NormalizeRequired(awayTeam, nameof(AwayTeam));
        MatchDate = matchDate;
        Stage = stage;
        Status = MatchStatus.Scheduled;
        Stadium = NormalizeOptional(stadium);
        CreatedAt = DateTime.UtcNow;

        ValidateTeams();
        ValidateMatchDate();
        ValidateStage();
        ValidateScoreState();
    }

    public Guid Id { get; private set; }
    public string HomeTeam { get; private set; }
    public string AwayTeam { get; private set; }
    public int? HomeScore { get; private set; }
    public int? AwayScore { get; private set; }
    public DateTime MatchDate { get; private set; }
    public MatchStage Stage { get; private set; }
    public MatchStatus Status { get; private set; }
    public string? Stadium { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static WorldCupMatch Create(
        string homeTeam,
        string awayTeam,
        DateTime matchDate,
        MatchStage stage,
        string? stadium)
    {
        return new WorldCupMatch(Guid.NewGuid(), homeTeam, awayTeam, matchDate, stage, stadium);
    }

    public void Update(
        string homeTeam,
        string awayTeam,
        DateTime matchDate,
        MatchStage stage,
        string? stadium)
    {
        HomeTeam = NormalizeRequired(homeTeam, nameof(HomeTeam));
        AwayTeam = NormalizeRequired(awayTeam, nameof(AwayTeam));
        MatchDate = matchDate;
        Stage = stage;
        Stadium = NormalizeOptional(stadium);

        ValidateTeams();
        ValidateMatchDate();
        ValidateStage();
        MarkUpdated();
    }

    public void UpdateScore(int homeScore, int awayScore)
    {
        HomeScore = homeScore;
        AwayScore = awayScore;

        ValidateScores();
        ValidateScoreState();
        MarkUpdated();
    }

    public void ChangeStatus(MatchStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new DomainException("Match status must be valid.");
        }

        if (Status == MatchStatus.Finished && status == MatchStatus.Scheduled)
        {
            throw new DomainException("A finished match cannot return to scheduled.");
        }

        if (Status == MatchStatus.Canceled && status == MatchStatus.Finished)
        {
            throw new DomainException("A canceled match cannot be marked as finished.");
        }

        Status = status;

        ValidateScoreState();
        MarkUpdated();
    }

    private static string NormalizeRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{fieldName} is required.");
        }

        return value.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private void ValidateTeams()
    {
        if (string.Equals(HomeTeam, AwayTeam, StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException("Home team and away team cannot be the same.");
        }
    }

    private void ValidateMatchDate()
    {
        if (MatchDate == default)
        {
            throw new DomainException("Match date is required.");
        }
    }

    private void ValidateStage()
    {
        if (!Enum.IsDefined(Stage))
        {
            throw new DomainException("Match stage must be valid.");
        }
    }

    private void ValidateScores()
    {
        if (HomeScore is < 0 || AwayScore is < 0)
        {
            throw new DomainException("Scores cannot be negative.");
        }
    }

    private void ValidateScoreState()
    {
        ValidateScores();

        if (Status == MatchStatus.Scheduled && (HomeScore.HasValue || AwayScore.HasValue))
        {
            throw new DomainException("Scheduled matches cannot have a score.");
        }

        if (Status == MatchStatus.Finished && (!HomeScore.HasValue || !AwayScore.HasValue))
        {
            throw new DomainException("Finished matches must have a score.");
        }
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
