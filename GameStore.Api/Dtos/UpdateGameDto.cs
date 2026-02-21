using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    [Range(1, 100)] decimal Price,
    [Required][StringLength(50)] string Genre
);
