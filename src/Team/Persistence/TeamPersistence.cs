using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Team.Transfer;
using Lsquad.TeamName;
using Lsquad.TeamName.Transfer;

namespace Lsquad.Team.Persistence;

public class TeamPersistence(ITeamNameService teamNameService) : LsquadPersistence, ITeamPersistence
{
    public void CreateOrUpdateByExternalTeamId(List<TeamTransfer> teamTransfers)
    {
        if (teamTransfers.Count == 0) { return; }
        Dictionary<int, List<TeamNameTransfer>> externalTeamIdToNameTransfers = new();
        foreach (TeamTransfer teamTransfer in teamTransfers)
        {
            externalTeamIdToNameTransfers[teamTransfer.external_team_id] = teamTransfer.teamNameTransfers;
        }

        teamTransfers = InsertIfNotExistsElseUpdateWithExternalId(teamTransfers);
        List<TeamNameTransfer> teamNameTransfers = [];
        foreach (TeamTransfer teamTransfer in teamTransfers)
        {
            foreach (TeamNameTransfer teamNameTransfer in externalTeamIdToNameTransfers[teamTransfer.external_team_id])
            {
                teamNameTransfer.fk_team = teamTransfer.id_team;
            }
            teamNameTransfers.AddRange(externalTeamIdToNameTransfers[teamTransfer.external_team_id]);
        }

        teamNameService.CreateOrUpdate(teamNameTransfers);
    }

    public TeamTransfer CreateOrUpdateByExternalTeamId(TeamTransfer teamTransfer)
    {
        List<TeamNameTransfer> teamNameTransfers = teamTransfer.teamNameTransfers;
        teamTransfer = InsertIfNotExistsElseUpdateWithExternalId(teamTransfer);
        if (teamNameTransfers.Count == 0)
        {
            return teamTransfer;
        }

        foreach (TeamNameTransfer teamNameTransfer in teamNameTransfers)
        {
            teamNameTransfer.fk_team = teamTransfer.id_team;
        }
        teamNameService.CreateOrUpdate(teamNameTransfers);

        return teamTransfer;
    }

    public List<TeamIdName> GetTeamIdNameBy(int externalTeamId, List<int> idLanguages)
    {
        const string sql =
            @"SELECT st.id_team as ""id_team"", stn.name as ""name"", stn.fk_language as ""fk_language"" FROM lsquad_team st " +
            " JOIN lsquad_team_name stn " +
            " ON st.id_team = stn.fk_team " +
            " WHERE stn.fk_language = ANY(@fk_languages) " +
            " AND st.external_team_id = @external_team_id ";
        var parameters = new { fk_languages = idLanguages, externalTeamId = externalTeamId };
        Console.WriteLine($"running {sql} with {parameters}");

        return GetConnection().Query<TeamIdName>(sql, parameters).ToList();
    }

    private List<TeamTransfer> InsertIfNotExistsElseUpdateWithExternalId(List<TeamTransfer> teamTransfers)
    {
        if (teamTransfers.Count == 0) { return []; }
        List<string> valuesWithBrackets = [];
        List<int> uniqueList = [];
        foreach (TeamTransfer teamTransfer in teamTransfers)
        {
            if (uniqueList.Contains(teamTransfer.external_team_id)) { continue; }
            valuesWithBrackets.Add("(" + teamTransfer.external_team_id.ToString() + ", now())");
            uniqueList.Add(teamTransfer.external_team_id);
        }

        string sql =
            $@"INSERT INTO lsquad_team (external_team_id, created_at) VALUES {String.Join(", ", valuesWithBrackets)} " +
            "  ON CONFLICT (external_team_id) DO UPDATE SET updated_at = now() " +
            "  RETURNING *";
        Console.WriteLine($"running {sql} with {teamTransfers.Count} teamTransfers");

        return GetConnection().Query<TeamTransfer>(sql).ToList();
    }

    private TeamTransfer InsertIfNotExistsElseUpdateWithExternalId(TeamTransfer teamTransfer)
    {
        const string sql =
            @"INSERT INTO lsquad_team (external_team_id, created_at) VALUES (@external_team_id, now()) " +
            " ON CONFLICT (external_team_id) DO UPDATE " +
            " set updated_at = now() " +
            " RETURNING *";
        Console.WriteLine($"running {sql} with {teamTransfer}");

        return GetConnection().QueryFirst<TeamTransfer>(sql, teamTransfer);
    }
}
