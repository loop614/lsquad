using Lsquad.TeamName.Persistence;
using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName;

public class TeamNameService(ITeamNamePersistence teamNamePersistence) : ITeamNameService
{
    public void CreateOrUpdate(List<TeamNameTransfer> teamNames)
    {
        teamNamePersistence.CreateOrUpdate(teamNames);
    }

    public string? GetTeamName(int idTeam, int idLanguage)
    {
        return teamNamePersistence.GetTeamName(idTeam, idLanguage);
    }
}
