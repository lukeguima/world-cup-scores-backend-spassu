using WorldCupScores.Application.Dtos;
using WorldCupScores.Domain.Enums;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Commands;

public sealed record ChangeWorldCupMatchStatusCommand(Guid Id, MatchStatus Status);

public sealed class ChangeWorldCupMatchStatusCommandHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public ChangeWorldCupMatchStatusCommandHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorldCupMatchDto?> HandleAsync(
        ChangeWorldCupMatchStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var match = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (match is null)
        {
            return null;
        }

        match.ChangeStatus(command.Status);
        await _repository.UpdateAsync(match, cancellationToken);

        return WorldCupMatchDto.FromEntity(match);
    }
}
