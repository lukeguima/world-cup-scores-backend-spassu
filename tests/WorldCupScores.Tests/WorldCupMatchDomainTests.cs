using FluentAssertions;
using WorldCupScores.Domain.Entities;
using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Exceptions;

namespace WorldCupScores.Tests;

public sealed class WorldCupMatchDomainTests
{
    [Fact]
    public void Should_create_valid_match()
    {
        var match = CreateMatch();

        match.Id.Should().NotBeEmpty();
        match.HomeTeam.Should().Be("Brazil");
        match.AwayTeam.Should().Be("France");
        match.Status.Should().Be(MatchStatus.Scheduled);
        match.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        match.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Should_not_create_match_with_empty_home_team()
    {
        var act = () => WorldCupMatch.Create(
            "",
            "France",
            DateTime.UtcNow,
            MatchStage.GroupStage,
            null);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_create_match_with_empty_away_team()
    {
        var act = () => WorldCupMatch.Create(
            "Brazil",
            " ",
            DateTime.UtcNow,
            MatchStage.GroupStage,
            null);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_allow_same_home_and_away_team()
    {
        var act = () => WorldCupMatch.Create(
            "Brazil",
            "brazil",
            DateTime.UtcNow,
            MatchStage.GroupStage,
            null);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_allow_negative_score()
    {
        var match = CreateMatch();
        match.ChangeStatus(MatchStatus.InProgress);

        var act = () => match.UpdateScore(-1, 0);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_allow_scheduled_match_with_score()
    {
        var match = CreateMatch();

        var act = () => match.UpdateScore(1, 0);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_allow_finished_match_without_score()
    {
        var match = CreateMatch();

        var act = () => match.ChangeStatus(MatchStatus.Finished);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_allow_finished_match_return_to_scheduled()
    {
        var match = CreateMatch();
        match.ChangeStatus(MatchStatus.InProgress);
        match.UpdateScore(2, 1);
        match.ChangeStatus(MatchStatus.Finished);

        var act = () => match.ChangeStatus(MatchStatus.Scheduled);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_not_allow_canceled_match_change_to_finished()
    {
        var match = CreateMatch();
        match.ChangeStatus(MatchStatus.Canceled);

        var act = () => match.ChangeStatus(MatchStatus.Finished);

        act.Should().Throw<DomainException>();
    }

    private static WorldCupMatch CreateMatch()
    {
        return WorldCupMatch.Create(
            "Brazil",
            "France",
            new DateTime(2026, 6, 15, 16, 0, 0),
            MatchStage.GroupStage,
            "Maracana");
    }
}
