using WorldCupScores.Application.Dtos;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Queries;

public sealed record GetWorldCupMatchesQuery;

public sealed class GetWorldCupMatchesQueryHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public GetWorldCupMatchesQueryHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<WorldCupMatchDto>> HandleAsync(
        GetWorldCupMatchesQuery query,
        CancellationToken cancellationToken = default)
    {
        var matches = await _repository.GetAllAsync(cancellationToken);

        return matches.Select(WorldCupMatchDto.FromEntity).ToList();
    }
}
