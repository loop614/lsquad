using Lsquad.Team.Persistence;
using Lsquad.Team.Transfer;

namespace Lsquad.Team;

public class TeamService(ITeamPersistence teamPersistence) : ITeamService
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamEntities)
    {
        teamPersistence.CreateOrUpdateByExternalTeamId(teamEntities);
    }

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamEntity)
    {
        return teamPersistence.CreateOrUpdateByExternalTeamId(teamEntity);
    }

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages)
    {
        return teamPersistence.GetTeamIdNameBy(externalTeamId, idLanguages);
    }
}
