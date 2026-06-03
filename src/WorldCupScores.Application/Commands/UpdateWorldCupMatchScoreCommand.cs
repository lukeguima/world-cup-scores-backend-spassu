using WorldCupScores.Application.Dtos;
using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Commands;

public sealed record UpdateWorldCupMatchScoreCommand(Guid Id, int HomeScore, int AwayScore);

public sealed class UpdateWorldCupMatchScoreCommandHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public UpdateWorldCupMatchScoreCommandHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorldCupMatchDto?> HandleAsync(
        UpdateWorldCupMatchScoreCommand command,
        CancellationToken cancellationToken = default)
    {
        var match = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (match is null)
        {
            return null;
        }

        if (match.Status == MatchStatus.Scheduled)
        {
            match.ChangeStatus(MatchStatus.InProgress);
        }

        match.UpdateScore(command.HomeScore, command.AwayScore);
        await _repository.UpdateAsync(match, cancellationToken);

        return WorldCupMatchDto.FromEntity(match);
    }
}
