using Lsquad.Team.Persistence;
using Lsquad.TeamName;

namespace Lsquad.Team;

public class TeamFactory
{
    public ITeamPersistence CreateTeamPersistence()
    {
        return new TeamPersistence(CreateTeamNameFacade());
    }

    private ITeamNameFacade CreateTeamNameFacade()
    {
        return new TeamNameFacade();
    }
}
