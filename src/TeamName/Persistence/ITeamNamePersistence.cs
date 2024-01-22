using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName.Persistence;

public interface ITeamNamePersistence
{
    public void CreateOrUpdate(List<TeamNameEntity> teamNames);

    public string GetTeamName(int idTeam, int idLanguage);
}
