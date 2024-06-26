using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName.Persistence;

public interface ITeamNamePersistence
{
    public void CreateOrUpdate(List<TeamNameTransfer> teamNames);

    public string? GetTeamName(int idTeam, int idLanguage);
}
