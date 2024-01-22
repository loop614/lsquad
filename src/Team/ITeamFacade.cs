using Lsquad.Team.Transfer;

namespace Lsquad.Team;

public interface ITeamFacade
{
    public void CreateOrUpdateByExternalTeamId(List<TeamEntity> teamEntities);

    public TeamEntity CreateOrUpdateByExternalTeamId(TeamEntity teamEntity);

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages);
}
