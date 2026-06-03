using Microsoft.AspNetCore.Mvc;
using WorldCupScores.Application.Commands;
using WorldCupScores.Application.Dtos;
using WorldCupScores.Application.Queries;

namespace WorldCupScores.Api.Controllers;

[ApiController]
[Route("api/world-cup-matches")]
public sealed class WorldCupMatchesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WorldCupMatchDto>>> GetAll(
        [FromServices] GetWorldCupMatchesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var matches = await handler.HandleAsync(new GetWorldCupMatchesQuery(), cancellationToken);

        return Ok(matches);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorldCupMatchDto>> GetById(
        Guid id,
        [FromServices] GetWorldCupMatchByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var match = await handler.HandleAsync(new GetWorldCupMatchByIdQuery(id), cancellationToken);

        return match is null ? NotFound() : Ok(match);
    }

    [HttpPost]
    public async Task<ActionResult<WorldCupMatchDto>> Create(
        CreateWorldCupMatchRequest request,
        [FromServices] CreateWorldCupMatchCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var match = await handler.HandleAsync(
            new CreateWorldCupMatchCommand(
                request.HomeTeam,
                request.AwayTeam,
                request.MatchDate,
                request.Stage,
                request.Stadium),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = match.Id }, match);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<WorldCupMatchDto>> Update(
        Guid id,
        UpdateWorldCupMatchRequest request,
        [FromServices] UpdateWorldCupMatchCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var match = await handler.HandleAsync(
            new UpdateWorldCupMatchCommand(
                id,
                request.HomeTeam,
                request.AwayTeam,
                request.MatchDate,
                request.Stage,
                request.Stadium),
            cancellationToken);

        return match is null ? NotFound() : Ok(match);
    }

    [HttpPatch("{id:guid}/score")]
    public async Task<ActionResult<WorldCupMatchDto>> UpdateScore(
        Guid id,
        UpdateWorldCupMatchScoreRequest request,
        [FromServices] UpdateWorldCupMatchScoreCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var match = await handler.HandleAsync(
            new UpdateWorldCupMatchScoreCommand(id, request.HomeScore, request.AwayScore),
            cancellationToken);

        return match is null ? NotFound() : Ok(match);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<WorldCupMatchDto>> ChangeStatus(
        Guid id,
        ChangeWorldCupMatchStatusRequest request,
        [FromServices] ChangeWorldCupMatchStatusCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var match = await handler.HandleAsync(
            new ChangeWorldCupMatchStatusCommand(id, request.Status),
            cancellationToken);

        return match is null ? NotFound() : Ok(match);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteWorldCupMatchCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var deleted = await handler.HandleAsync(new DeleteWorldCupMatchCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
