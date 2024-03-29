using Lsquad.Team.Transfer;

namespace Lsquad.Team.Persistence;

public interface ITeamPersistence
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamTransfer);

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamTransfer);

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages);
}
