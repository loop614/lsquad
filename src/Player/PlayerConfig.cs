using Lsquad.Player.Persistence;

namespace Lsquad.Player;

public class PlayerConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPlayerPersistence, PlayerPersistence>();
        builder.Services.AddTransient<IPlayerService, PlayerService>();
    }
}
