using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Team.Transfer;
using Lsquad.TeamName;
using Lsquad.TeamName.Transfer;

namespace Lsquad.Team.Persistence;

public class TeamPersistence(ITeamNameFacade teamNameFacade) : LsquadPersistence, ITeamPersistence
{
    private readonly ITeamNameFacade _teamNameFacade = teamNameFacade;

    public void CreateOrUpdateByExternalTeamId(List<TeamEntity> teamEntities)
    {
        if (teamEntities.Count == 0) {return;}
        Dictionary<int, List<TeamNameEntity>> externalTeamIdToNameEntities = [];
        foreach(TeamEntity teamEntity in teamEntities) {
            externalTeamIdToNameEntities[teamEntity.external_team_id] = teamEntity.teamNameEntities;
        }

        teamEntities = InsertIfNotExistsElseUpdateWithExternalId(teamEntities);
        List<TeamNameEntity> teamNameEntities = [];
        foreach(TeamEntity teamEntity in teamEntities) {
            foreach(TeamNameEntity teamNameEntity in externalTeamIdToNameEntities[teamEntity.external_team_id]) {
                teamNameEntity.fk_team = teamEntity.id_team;
            }
            teamNameEntities.AddRange(externalTeamIdToNameEntities[teamEntity.external_team_id]);
        }

        _teamNameFacade.CreateOrUpdate(teamNameEntities);
    }

    public TeamEntity CreateOrUpdateByExternalTeamId(TeamEntity teamEntity)
    {
        List<TeamNameEntity> teamNameEntities = teamEntity.teamNameEntities;
        teamEntity = InsertIfNotExistsElseUpdateWithExternalId(teamEntity);
        if (teamNameEntities.Count == 0) {
            return teamEntity;
        }

        foreach(TeamNameEntity teamNameEntity in teamNameEntities) {
            teamNameEntity.fk_team = teamEntity.id_team;
        }
        _teamNameFacade.CreateOrUpdate(teamNameEntities);

        return teamEntity;
    }

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages)
    {
        const string sql =
            @"SELECT st.id_team as ""id_team"", stn.name as ""name"", stn.fk_language as ""fk_language"" FROM lsquad_team st " +
            " JOIN lsquad_team_name stn " +
            " ON st.id_team = stn.fk_team " +
            " WHERE stn.fk_language = ANY(@fk_languages) " +
            " AND st.external_team_id = @external_team_id ";
        var parameters = new {fk_languages = idLanguages, external_team_id = externalTeamId};
        Console.WriteLine($"running {sql} with {parameters}");

        return GetConnection().Query<TeamIdName>(sql, parameters).ToList();
    }

    private List<TeamEntity> InsertIfNotExistsElseUpdateWithExternalId(List<TeamEntity> teamEntities)
    {
        if (teamEntities.Count == 0) {return [];}
        List<string> valuesWithBrackets = [];
        List<int> uniqueList = [];
        foreach(TeamEntity teamEntity in teamEntities) {
            if (uniqueList.Contains(teamEntity.external_team_id)) {continue;}
            valuesWithBrackets.Add("(" + teamEntity.external_team_id.ToString() + ", now())");
            uniqueList.Add(teamEntity.external_team_id);
        }

        string sql =
            $@"INSERT INTO lsquad_team (external_team_id, created_at) VALUES {String.Join(", ", valuesWithBrackets)} " +
            "  ON CONFLICT (external_team_id) DO UPDATE SET updated_at = now() " +
            "  RETURNING *";
        Console.WriteLine($"running {sql} with {teamEntities.Count} teamEntities");

        return GetConnection().Query<TeamEntity>(sql).ToList();
    }

    private TeamEntity InsertIfNotExistsElseUpdateWithExternalId(TeamEntity teamEntity)
    {
        const string sql =
            @"INSERT INTO lsquad_team (external_team_id, created_at) VALUES (@external_team_id, now()) " +
            " ON CONFLICT (external_team_id) DO UPDATE " +
            " set updated_at = now() " +
            " RETURNING *";
        Console.WriteLine($"running {sql} with {teamEntity}");

        return GetConnection().QueryFirst<TeamEntity>(sql, teamEntity);
    }
}


/*
2) if we land all versions in the same table
SELECT st.id_team as ""id_team"", stn.name as ""name"", stn.fk_language as ""fk_language"" FROM lsquad_team st
FROM lsquad_team st
JOIN lsquad_team_name stn ON stn.fk_team = st.id_team
WHERE (st.id_team, stn.version) IN (
    SELECT id_team, MAX(stn.version) AS max_version
    FROM lsquad_team st
    JOIN lsquad_team_name spn ON stn.fk_team = st.id_team
    WHERE st.fk_team = @fk_team
    AND stn.fk_language = ANY (@fk_language)
    GROUP BY id_team
);
*/
