using Dapper;
using Npgsql;

namespace Lsquad.Core.Persistence;

class LsquadCoreDatabaseClean
{
    public async static Task DropTables(NpgsqlConnection connection) {
        var sql = """
            DROP TABLE IF EXISTS lsquad_setting;
            DROP TABLE IF EXISTS lsquad_player_name;
            DROP TABLE IF EXISTS lsquad_player;
            DROP TABLE IF EXISTS lsquad_team_name;
            DROP TABLE IF EXISTS lsquad_team;
            DROP TABLE IF EXISTS lsquad_language;
        """;

        await connection.ExecuteAsync(sql);
    }
}
