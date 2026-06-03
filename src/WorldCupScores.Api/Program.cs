using WorldCupScores.Application.Commands;
using WorldCupScores.Application.Queries;
using WorldCupScores.Domain.Exceptions;
using WorldCupScores.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                string.Equals(
                    origin.TrimEnd('/'),
                    "http://localhost:5173",
                    StringComparison.OrdinalIgnoreCase))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<CreateWorldCupMatchCommandHandler>();
builder.Services.AddScoped<UpdateWorldCupMatchCommandHandler>();
builder.Services.AddScoped<UpdateWorldCupMatchScoreCommandHandler>();
builder.Services.AddScoped<ChangeWorldCupMatchStatusCommandHandler>();
builder.Services.AddScoped<DeleteWorldCupMatchCommandHandler>();
builder.Services.AddScoped<GetWorldCupMatchesQueryHandler>();
builder.Services.AddScoped<GetWorldCupMatchByIdQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?
            .Error;

        context.Response.ContentType = "application/json";

        if (exception is DomainException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = exception.Message });
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    });
});

app.UseCors("Frontend");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
