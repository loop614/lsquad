using Lsquad.Language;
using Lsquad.Player;
using Lsquad.PlayerName;
using Lsquad.Squad.Domain;
using Lsquad.Team;
using Lsquad.TeamName;

namespace Lsquad.Squad;

public class SquadFactory
{
    public ISquadReader CreateSquadReader()
    {
        return new SquadReader(
            CreateLanguageFacade(),
            CreateTeamFacade(),
            CreateTeamNameFacade(),
            CreatePlayerFacade(),
            CreatePlayerNameFacade()
        );
    }

    private IPlayerNameFacade CreatePlayerNameFacade()
    {
        return new PlayerNameFacade();
    }

    private IPlayerFacade CreatePlayerFacade()
    {
        return new PlayerFacade();
    }

    private ITeamNameFacade CreateTeamNameFacade()
    {
        return new TeamNameFacade();
    }

    private ITeamFacade CreateTeamFacade()
    {
        return new TeamFacade();
    }

    private ILanguageFacade CreateLanguageFacade()
    {
        return new LanguageFacade();
    }
}
