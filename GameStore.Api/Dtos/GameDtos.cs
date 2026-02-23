using System.ComponentModel.DataAnnotations;
using GameStore.Api.Models;

namespace GameStore.Api.Dtos;

public record class GameDtos(
    int Id,
    string Title,
    string Description,
    decimal Price,
    int GenreId,
    string GenreName
);

