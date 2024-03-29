using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Team.Transfer;
using Lsquad.TeamName;
using Lsquad.TeamName.Transfer;

namespace Lsquad.Team.Persistence;

public class TeamPersistence(ITeamNameService teamNameService) : LsquadPersistence, ITeamPersistence
{
    private readonly ITeamNameService _teamNameService = teamNameService;

    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamEntities)
    {
        if (teamEntities.Count == 0) {return;}
        Dictionary<int, List<TeamNameTransfer>> externalTeamIdToNameEntities = [];
        foreach(TeamTransfer teamEntity in teamEntities) {
            externalTeamIdToNameEntities[teamEntity.externalTeamId] = teamEntity.teamNameEntities;
        }

        teamEntities = InsertIfNotExistsElseUpdateWithExternalId(teamEntities);
        List<TeamNameTransfer> teamNameEntities = [];
        foreach(TeamTransfer teamEntity in teamEntities) {
            foreach(TeamNameTransfer teamNameTransfer in externalTeamIdToNameEntities[teamEntity.externalTeamId]) {
                teamNameTransfer.fk_team = teamEntity.id_team;
            }
            teamNameEntities.AddRange(externalTeamIdToNameEntities[teamEntity.externalTeamId]);
        }

        _teamNameService.CreateOrUpdate(teamNameEntities);
    }

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamEntity)
    {
        List<TeamNameTransfer> teamNameEntities = teamEntity.teamNameEntities;
        teamEntity = InsertIfNotExistsElseUpdateWithExternalId(teamEntity);
        if (teamNameEntities.Count == 0) {
            return teamEntity;
        }

        foreach(TeamNameTransfer teamNameTransfer in teamNameEntities) {
            teamNameTransfer.fk_team = teamEntity.id_team;
        }
        _teamNameService.CreateOrUpdate(teamNameEntities);

        return teamEntity;
    }

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages)
    {
        const string sql =
            @"SELECT st.id_team as ""id_team"", stn.name as ""name"", stn.fk_language as ""fk_language"" FROM lsquad_team st " +
            " JOIN lsquad_team_name stn " +
            " ON st.id_team = stn.fk_team " +
            " WHERE stn.fk_language = ANY(@fk_languages) " +
            " AND st.externalTeamId = @externalTeamId ";
        var parameters = new {fk_languages = idLanguages, externalTeamId = externalTeamId};
        Console.WriteLine($"running {sql} with {parameters}");

        return GetConnection().Query<TeamIdName>(sql, parameters).ToList();
    }

    private List<TeamTransfer> InsertIfNotExistsElseUpdateWithExternalId(List<TeamTransfer> teamEntities)
    {
        if (teamEntities.Count == 0) {return [];}
        List<string> valuesWithBrackets = [];
        List<int> uniqueList = [];
        foreach(TeamTransfer teamEntity in teamEntities) {
            if (uniqueList.Contains(teamEntity.externalTeamId)) {continue;}
            valuesWithBrackets.Add("(" + teamEntity.externalTeamId.ToString() + ", now())");
            uniqueList.Add(teamEntity.externalTeamId);
        }

        string sql =
            $@"INSERT INTO lsquad_team (externalTeamId, created_at) VALUES {String.Join(", ", valuesWithBrackets)} " +
            "  ON CONFLICT (externalTeamId) DO UPDATE SET updated_at = now() " +
            "  RETURNING *";
        Console.WriteLine($"running {sql} with {teamEntities.Count} teamEntities");

        return GetConnection().Query<TeamTransfer>(sql).ToList();
    }

    private TeamTransfer InsertIfNotExistsElseUpdateWithExternalId(TeamTransfer teamEntity)
    {
        const string sql =
            @"INSERT INTO lsquad_team (externalTeamId, created_at) VALUES (@externalTeamId, now()) " +
            " ON CONFLICT (externalTeamId) DO UPDATE " +
            " set updated_at = now() " +
            " RETURNING *";
        Console.WriteLine($"running {sql} with {teamEntity}");

        return GetConnection().QueryFirst<TeamTransfer>(sql, teamEntity);
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
