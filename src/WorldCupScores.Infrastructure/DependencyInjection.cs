using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorldCupScores.Domain.Repositories;
using WorldCupScores.Infrastructure.Persistence;
using WorldCupScores.Infrastructure.Repositories;

namespace WorldCupScores.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IWorldCupMatchRepository, WorldCupMatchRepository>();

        return services;
    }
}
