using WorldCupScores.Application.Dtos;
using WorldCupScores.Domain.Repositories;

namespace WorldCupScores.Application.Queries;

public sealed record GetWorldCupMatchByIdQuery(Guid Id);

public sealed class GetWorldCupMatchByIdQueryHandler
{
    private readonly IWorldCupMatchRepository _repository;

    public GetWorldCupMatchByIdQueryHandler(IWorldCupMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorldCupMatchDto?> HandleAsync(
        GetWorldCupMatchByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var match = await _repository.GetByIdAsync(query.Id, cancellationToken);

        return match is null ? null : WorldCupMatchDto.FromEntity(match);
    }
}
