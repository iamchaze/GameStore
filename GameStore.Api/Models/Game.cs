using System;

namespace GameStore.Api.Models;

public class Game
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required Genre Genre { get; set; }
    public int GenreId { get; set; }
}
