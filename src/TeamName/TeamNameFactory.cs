using Lsquad.TeamName.Persistence;

namespace Lsquad.TeamName;

public class TeamNameFactory
{
    public ITeamNamePersistence CreateTeamNamePersistence()
    {
        return new TeamNamePersistence();
    }
}
