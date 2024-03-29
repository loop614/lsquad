using Lsquad.Team.Transfer;

namespace Lsquad.Team.Persistence;

public interface ITeamPersistence
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamEntity);

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamEntity);

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages);
}
