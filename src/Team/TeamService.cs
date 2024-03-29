using Lsquad.Team.Persistence;
using Lsquad.Team.Transfer;

namespace Lsquad.Team;

public class TeamService(ITeamPersistence teamPersistence) : ITeamService
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamTransfers)
    {
        teamPersistence.CreateOrUpdateByExternalTeamId(teamTransfers);
    }

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamTransfer)
    {
        return teamPersistence.CreateOrUpdateByExternalTeamId(teamTransfer);
    }

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages)
    {
        return teamPersistence.GetTeamIdNameBy(externalTeamId, idLanguages);
    }
}
