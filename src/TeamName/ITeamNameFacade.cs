using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName;

public interface ITeamNameFacade
{
    public void CreateOrUpdate(List<TeamNameEntity> teamNames);

    public string GetTeamName(int idTeam, int idLanguage);
}
