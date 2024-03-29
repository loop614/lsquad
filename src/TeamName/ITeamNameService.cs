using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName;

public interface ITeamNameService
{
    public void CreateOrUpdate(List<TeamNameTransfer> teamNames);

    public string? GetTeamName(int idTeam, int idLanguage);
}
