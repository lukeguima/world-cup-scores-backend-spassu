using WorldCupScores.Application.Dtos;
using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Commands;

public sealed record UpdateWorldCupMatchCommand(
    Guid Id,
    string HomeTeam,
    string AwayTeam,
    DateTime MatchDate,
    MatchStage Stage,
    string? Stadium);

public sealed class UpdateWorldCupMatchCommandHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public UpdateWorldCupMatchCommandHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorldCupMatchDto?> HandleAsync(
        UpdateWorldCupMatchCommand command,
        CancellationToken cancellationToken = default)
    {
        var match = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (match is null)
        {
            return null;
        }

        match.Update(command.HomeTeam, command.AwayTeam, command.MatchDate, command.Stage, command.Stadium);
        await _repository.UpdateAsync(match, cancellationToken);

        return WorldCupMatchDto.FromEntity(match);
    }
}
