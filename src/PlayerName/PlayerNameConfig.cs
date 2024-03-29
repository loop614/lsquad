namespace Lsquad.PlayerName;

public class PlayerNameConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPlayerNamePersistence, PlayerNamePersistence>();
        builder.Services.AddTransient<IPlayerNameService, PlayerNameService>();
    }
}
