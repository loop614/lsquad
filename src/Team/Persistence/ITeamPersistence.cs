using Lsquad.Team.Transfer;

namespace Lsquad.Team.Persistence;

public interface ITeamPersistence
{
    public void CreateOrUpdateByExternalTeamId(List<TeamEntity> teamEntity);

    public TeamEntity CreateOrUpdateByExternalTeamId(TeamEntity teamEntity);

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages);
}
