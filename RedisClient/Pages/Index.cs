using RedisClient.Services;
using Microsoft.AspNetCore.Components;

namespace RedisClient.Pages;

public partial class Index : ComponentBase
{
    /// <summary>
    /// Gets or sets a flag indicating whether the data was loaded from the cache.
    /// </summary>
    private bool IsFromCache { get; set; } = false;

    /// <summary>
    /// Gets or sets the message to be displayed.
    /// </summary>
    private string? Message { get; set; }

    /// <summary>
    /// Gets or sets the list of games.
    /// </summary>
    private List<Game>? _games;

    [Inject]
    private IRedisService Cache { get; set; }

    [Inject]
    private IGameService GameService { get; set; }

    /// <summary>
    /// Initializes the component and fetches data.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await FetchData();
    }

    /// <summary>
    /// Fetches game data, either from the cache or the API.
    /// </summary>
    private async Task FetchData()
    {
        _games = await Cache.Get<List<Game>>("Games_Cache");

        if (_games == null)
        {
            _games = GameService.FetchData();
            await Cache.Set("Games_Cache", _games, TimeSpan.FromSeconds(60));
            Message = "Data loaded from the API";
            IsFromCache = false;
        }
        else
        {
            Message = "Data loaded from the cache";
            IsFromCache = true;
        }
    }

    /// <summary>
    /// Clears the cache and resets the message.
    /// </summary>
    private async Task ClearCache()
    {
        await Cache.Delete("Games_Cache");
        _games?.Clear();
        Message = string.Empty;
        StateHasChanged();
    }
}