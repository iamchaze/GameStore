using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";

    // private static readonly List<GameDtos> games = [
    // new(1,"mk1", "A fast-paced fighting game", 29.99m, 1),
    // new(2,"mk2", "An enhanced version of the original", 39.99m, 1),
    // new(3,"mk3", "The ultimate fighting experience", 49.99m, 1)
    // ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
            {
                var games = await dbContext
                    .Games.Select(game => new GameDtos(
                        game.Id,
                        game.Title,
                        game.Description,
                        game.Price,
                        game.GenreId,
                        game.Genre != null ? game.Genre.Name : string.Empty
                    ))
                    .ToListAsync();

                return Results.Ok(games);
            }
        );

        group
            .MapGet(
                "/{id}",
                async (int id, GameStoreContext dbContext) =>
                {
                    var game = await dbContext.Games.FindAsync(id);
                    return game is null
                        ? Results.NotFound()
                        : Results.Ok(
                            new GameDtos(
                                game.Id,
                                game.Title,
                                game.Description,
                                game.Price,
                                game.GenreId,
                                game.Genre != null ? game.Genre.Name : string.Empty
                            )
                        );
                }
            )
            .WithName(GetGameEndpointName);

        group.MapPost(
            "/",
            async (CreateGameDto createGameDto, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Title = createGameDto.Title,
                    Description = createGameDto.Description,
                    Price = createGameDto.Price,
                    GenreId = createGameDto.GenreId,
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDtos gameDtos = new(
                    game.Id,
                    game.Title,
                    game.Description,
                    game.Price,
                    game.GenreId,
                    game.Genre != null ? game.Genre.Name : string.Empty
                );
                return Results.CreatedAtRoute(
                    GetGameEndpointName,
                    new { id = gameDtos.Id },
                    gameDtos
                );
            }
        );

        group.MapPut(
            "/{id}",
            async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                if (game is null)
                {
                    return Results.NotFound();
                }
                game.Title = updatedGame.Title;
                game.Description = updatedGame.Description;
                game.Price = updatedGame.Price;
                game.GenreId = updatedGame.GenreId;
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
        );

        group.MapDelete(
            "/{id}",
            async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                if (game is null)
                {
                    return Results.NotFound();
                }
                dbContext.Games.Remove(game);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
        );
    }
};
