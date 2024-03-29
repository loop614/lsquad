using Lsquad.Squad.Domain;

namespace Lsquad.Squad;

public class SquadConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ISquadReader, SquadReader>();
        builder.Services.AddTransient<ISquadService, SquadService>();
    }
}
