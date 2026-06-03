using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Commands;

public sealed record DeleteWorldCupMatchCommand(Guid Id);

public sealed class DeleteWorldCupMatchCommandHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public DeleteWorldCupMatchCommandHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(
        DeleteWorldCupMatchCommand command,
        CancellationToken cancellationToken = default)
    {
        var match = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (match is null)
        {
            return false;
        }

        await _repository.DeleteAsync(match, cancellationToken);

        return true;
    }
}
