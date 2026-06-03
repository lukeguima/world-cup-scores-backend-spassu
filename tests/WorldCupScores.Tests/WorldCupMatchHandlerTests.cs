using FluentAssertions;
using WorldCupScores.Application.Commands;
using WorldCupScores.Application.Queries;
using WorldCupScores.Domain.Entities;
using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Tests;

public sealed class WorldCupMatchHandlerTests
{
    [Fact]
    public async Task CreateWorldCupMatchCommandHandler_should_create_match_correctly()
    {
        var repository = new FakeWorldCupMatchRepository();
        var handler = new CreateWorldCupMatchCommandHandler(repository);

        var result = await handler.HandleAsync(new CreateWorldCupMatchCommand(
            "Brazil",
            "Argentina",
            new DateTime(2026, 7, 10, 20, 0, 0),
            MatchStage.Final,
            "MetLife Stadium"));

        result.Id.Should().NotBeEmpty();
        result.HomeTeam.Should().Be("Brazil");
        result.AwayTeam.Should().Be("Argentina");
        result.Status.Should().Be(MatchStatus.Scheduled);
        repository.Items.Should().ContainSingle(match => match.Id == result.Id);
    }

    [Fact]
    public async Task GetWorldCupMatchesQueryHandler_should_return_matches()
    {
        var repository = new FakeWorldCupMatchRepository();
        await repository.AddAsync(WorldCupMatch.Create(
            "Brazil",
            "Argentina",
            new DateTime(2026, 7, 10, 20, 0, 0),
            MatchStage.Final,
            "MetLife Stadium"));
        await repository.AddAsync(WorldCupMatch.Create(
            "France",
            "Germany",
            new DateTime(2026, 7, 9, 20, 0, 0),
            MatchStage.SemiFinal,
            null));

        var handler = new GetWorldCupMatchesQueryHandler(repository);

        var result = await handler.HandleAsync(new GetWorldCupMatchesQuery());

        result.Should().HaveCount(2);
        result.Select(match => match.HomeTeam).Should().Contain(["Brazil", "France"]);
    }

    [Fact]
    public async Task UpdateWorldCupMatchScoreCommandHandler_should_move_scheduled_match_to_in_progress()
    {
        var repository = new FakeWorldCupMatchRepository();
        var match = WorldCupMatch.Create(
            "Uruguay",
            "Bolivia",
            new DateTime(2026, 6, 3, 19, 0, 0, DateTimeKind.Utc),
            MatchStage.SemiFinal,
            null);
        await repository.AddAsync(match);

        var handler = new UpdateWorldCupMatchScoreCommandHandler(repository);

        var result = await handler.HandleAsync(new UpdateWorldCupMatchScoreCommand(match.Id, 0, 2));

        result.Should().NotBeNull();
        result!.Status.Should().Be(MatchStatus.InProgress);
        result.HomeScore.Should().Be(0);
        result.AwayScore.Should().Be(2);
    }

    private sealed class FakeWorldCupMatchRepository : IWorldCupMatchRepository
    {
        private readonly List<WorldCupMatch> _items = [];

        public IReadOnlyList<WorldCupMatch> Items => _items;

        public Task<IReadOnlyList<WorldCupMatch>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<WorldCupMatch>>(_items.OrderBy(match => match.MatchDate).ToList());
        }

        public Task<WorldCupMatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(match => match.Id == id));
        }

        public Task AddAsync(WorldCupMatch match, CancellationToken cancellationToken = default)
        {
            _items.Add(match);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(WorldCupMatch match, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(WorldCupMatch match, CancellationToken cancellationToken = default)
        {
            _items.Remove(match);
            return Task.CompletedTask;
        }
    }
}
