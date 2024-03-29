using Lsquad.Team.Transfer;

namespace Lsquad.Team;

public interface ITeamService
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamEntities);

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamEntity);

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages);
}
