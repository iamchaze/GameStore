namespace GameStore.Api.Dtos;

public record class GameDtos(
    int Id,
    string Title,
    string Description,
    decimal Price,
    string Genre
);

