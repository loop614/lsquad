using Lsquad.TeamName.Persistence;

namespace Lsquad.TeamName;

public class TeamNameConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITeamNamePersistence, TeamNamePersistence>();
        builder.Services.AddTransient<ITeamNameService, TeamNameService>();
    }
}
