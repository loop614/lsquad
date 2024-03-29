using Lsquad.Team.Persistence;

namespace Lsquad.Team;

public class TeamConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITeamPersistence, TeamPersistence>();
        builder.Services.AddTransient<ITeamService, TeamService>();
    }
}
