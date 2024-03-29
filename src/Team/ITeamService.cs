using Lsquad.Team.Transfer;

namespace Lsquad.Team;

public interface ITeamService
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamTransfers);

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamTransfer);

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages);
}
