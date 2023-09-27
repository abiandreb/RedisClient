namespace RedisClient.Services;

public class GameService : IGameService
{
    public List<Game> FetchData()
    {
        var games = new List<Game>()
        {
            new Game
            {
                Id = 1,
                Title = "The Witcher 3: Wild Hunt",
                Genre = "Action RPG",
                Platform = "PlayStation 4",
                ReleaseYear = 2015
            },
            new Game
            {
                Id = 2,
                Title = "Red Dead Redemption 2",
                Genre = "Action Adventure",
                Platform = "Xbox One",
                ReleaseYear = 2018
            },
            new Game
            {
                Id = 3,
                Title = "The Legend of Zelda: Breath of the Wild",
                Genre = "Action-Adventure",
                Platform = "Nintendo Switch",
                ReleaseYear = 2017
            },
            new Game
            {
                Id = 4,
                Title = "Cyberpunk 2077",
                Genre = "Action RPG",
                Platform = "PlayStation 5",
                ReleaseYear = 2020
            },
            new Game
            {
                Id = 5,
                Title = "Grand Theft Auto V",
                Genre = "Action-Adventure",
                Platform = "PC",
                ReleaseYear = 2013
            }
        };

        return games;
    }
}

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
}