using WorldCupScores.Domain.Entities;

namespace WorldCupScores.Domain.Repositories;

public interface IWorldCupMatchRepository
{
    Task<IReadOnlyList<WorldCupMatch>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<WorldCupMatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(WorldCupMatch match, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorldCupMatch match, CancellationToken cancellationToken = default);
    Task DeleteAsync(WorldCupMatch match, CancellationToken cancellationToken = default);
}
