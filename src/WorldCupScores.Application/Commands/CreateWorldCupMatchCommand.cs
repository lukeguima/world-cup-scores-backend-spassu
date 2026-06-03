using WorldCupScores.Application.Dtos;
using WorldCupScores.Domain.Entities;
using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Commands;

public sealed record CreateWorldCupMatchCommand(
    string HomeTeam,
    string AwayTeam,
    DateTime MatchDate,
    MatchStage Stage,
    string? Stadium);

public sealed class CreateWorldCupMatchCommandHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public CreateWorldCupMatchCommandHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorldCupMatchDto> HandleAsync(
        CreateWorldCupMatchCommand command,
        CancellationToken cancellationToken = default)
    {
        var match = WorldCupMatch.Create(
            command.HomeTeam,
            command.AwayTeam,
            command.MatchDate,
            command.Stage,
            command.Stadium);

        await _repository.AddAsync(match, cancellationToken);

        return WorldCupMatchDto.FromEntity(match);
    }
}
