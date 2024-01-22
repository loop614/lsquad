using Lsquad.Team.Transfer;

namespace Lsquad.Team;

public class TeamFacade : ITeamFacade
{
    private readonly TeamFactory _factory;

    public TeamFacade()
    {
        _factory = new TeamFactory();
    }

    public void CreateOrUpdateByExternalTeamId(List<TeamEntity> teamEntities)
    {
        _factory.CreateTeamPersistence().CreateOrUpdateByExternalTeamId(teamEntities);
    }

    public TeamEntity CreateOrUpdateByExternalTeamId(TeamEntity teamEntity)
    {
        return _factory.CreateTeamPersistence().CreateOrUpdateByExternalTeamId(teamEntity);
    }

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages)
    {
        return _factory.CreateTeamPersistence().GetTeamIdNameBy(externalTeamId, idLanguages);
    }
}
