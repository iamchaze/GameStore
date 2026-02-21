using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";
    private static readonly List<GameDtos> games = [
    new(1,"mk1", "A fast-paced fighting game", 29.99m, "Fighting"),
    new(2,"mk2", "An enhanced version of the original", 39.99m, "Fighting"),
    new(3,"mk3", "The ultimate fighting experience", 49.99m, "Fighting")
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");


        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto createGameDto) =>
        {
            // if (string.IsNullOrEmpty(createGameDto.Title))
            // {
            //     return Results.BadRequest("Title is required.");
            // }
            var newGame = new GameDtos(
                Id: games.Count + 1,
                Title: createGameDto.Title,
                Description: createGameDto.Description,
                Price: createGameDto.Price,
                Genre: createGameDto.Genre
            );
            games.Add(newGame);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = newGame.Id }, newGame);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = games[index] with
            {
                Title = updatedGame.Title,
                Description = updatedGame.Description,
                Price = updatedGame.Price,
                Genre = updatedGame.Genre
            };
            return Results.NoContent();

        });

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();

        });
    }
};
