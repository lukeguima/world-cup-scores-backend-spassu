using Microsoft.EntityFrameworkCore;
using WorldCupScores.Domain.Entities;
using WorldCupScores.Domain.Repositories;
using WorldCupScores.Infrastructure.Persistence;

namespace WorldCupScores.Infrastructure.Repositories;

public sealed class WorldCupMatchRepository : IWorldCupMatchRepository
{
    private readonly AppDbContext _dbContext;

    public WorldCupMatchRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<WorldCupMatch>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.WorldCupMatches
            .OrderBy(match => match.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorldCupMatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.WorldCupMatches.FirstOrDefaultAsync(match => match.Id == id, cancellationToken);
    }

    public async Task AddAsync(WorldCupMatch match, CancellationToken cancellationToken = default)
    {
        await _dbContext.WorldCupMatches.AddAsync(match, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WorldCupMatch match, CancellationToken cancellationToken = default)
    {
        _dbContext.WorldCupMatches.Update(match);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(WorldCupMatch match, CancellationToken cancellationToken = default)
    {
        _dbContext.WorldCupMatches.Remove(match);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
