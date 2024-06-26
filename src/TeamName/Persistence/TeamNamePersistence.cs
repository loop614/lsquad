using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.TeamName.Transfer;

namespace Lsquad.TeamName.Persistence;

public class TeamNamePersistence : LsquadPersistence, ITeamNamePersistence
{
    public void CreateOrUpdate(List<TeamNameTransfer> teamNameTransfers)
    {
        const string sql =
            @"INSERT INTO lsquad_team_name (fk_language, fk_team, version, name, created_at) VALUES (@fk_language, @fk_team, @version, @name, now()) " +
            " ON CONFLICT (fk_team, fk_language) DO UPDATE " +
            " set name = @name, version = @version, updated_at = now() ";

        Console.WriteLine($"running {sql} for {teamNameTransfers.Count} teamNameTransfers");

        GetConnection().Execute(sql, teamNameTransfers);
    }

    public string? GetTeamName(int idTeam, int idLanguage)
    {
        const string sql = @"SELECT name FROM lsquad_team_name WHERE fk_team = @idTeam AND fk_language = @idLanguage";
        var parameters = new { idTeam, idLanguage };
        Console.WriteLine($"running {sql} with {parameters}");

        return GetReadConnection().QueryFirst<TeamNameTransfer>(sql, parameters).name;
    }
}
