using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName;

public class TeamNameFacade : ITeamNameFacade
{
    private TeamNameFactory _factory;

    public TeamNameFacade()
    {
        _factory = new TeamNameFactory();
    }

    public void CreateOrUpdate(List<TeamNameEntity> teamNames)
    {
        _factory.CreateTeamNamePersistence().CreateOrUpdate(teamNames);
    }

    public string GetTeamName(int idTeam, int idLanguage)
    {
        return _factory.CreateTeamNamePersistence().GetTeamName(idTeam, idLanguage);
    }
}
