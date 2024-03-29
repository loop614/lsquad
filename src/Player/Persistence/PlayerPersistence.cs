using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Player.Transfer;
using Lsquad.PlayerName;

namespace Lsquad.Player.Persistence;

public class PlayerPersistence(IPlayerNameService playerNameService) : LsquadPersistence, IPlayerPersistence
{
    public void CreateOrUpdate(List<PlayerTransfer> playerEntities)
    {
        if (playerEntities.Count == 0) { return; }
        const string sql =
            @"INSERT INTO lsquad_player" +
            " (fk_team, external_player_id, weight, height, position, shirt_number, preferred_foot, version, full_name, first_name, last_name, country_code, birth_date, created_at) " +
            " VALUES " +
            " (@fk_team, @external_player_id, @weight, @height, @position, @shirt_number, @preferred_foot, @version, @full_name, @first_name, @last_name, @country_code, @birth_date, @created_at) " +
            " ON CONFLICT (external_player_id) DO UPDATE " +
            " set fk_team = @fk_team, external_player_id = @external_player_id, weight = @weight, height = @height, position = @position, shirt_number = @shirt_number, preferred_foot = @preferred_foot, version = @version, full_name = @full_name, first_name = @first_name, last_name = @last_name, country_code = @country_code, birth_date = @birth_date, updated_at = now() ";

        Console.WriteLine($"running {sql} with {playerEntities.Count} playerEntities");
        GetConnection().Execute(sql, playerEntities);
    }

    public void CreateOrUpdateWithExternalId(List<PlayerTransfer> playerEntities)
    {
        if (playerEntities.Count == 0) { return; }
        Dictionary<int, List<PlayerNameTransfer>> externalPlayerIdToNameEntities = [];
        foreach (PlayerTransfer playerTransfer in playerEntities)
        {
            externalPlayerIdToNameEntities[playerTransfer.external_player_id] = playerTransfer.playerNameEntities;
        }
        playerEntities = InsertIfNotExistsElseUpdateWithExternalId(playerEntities);
        List<PlayerNameTransfer> playerNameEntities = [];
        foreach (PlayerTransfer playerTransfer in playerEntities)
        {
            foreach (PlayerNameTransfer playerNameEntity in externalPlayerIdToNameEntities[playerTransfer.external_player_id])
            {
                playerNameEntity.fk_player = playerTransfer.id_player;
            }
            playerNameEntities.AddRange(externalPlayerIdToNameEntities[playerTransfer.external_player_id]);
        }
        playerNameService.CreateOrUpdate(playerNameEntities);
    }

    public List<PlayerTransferWithName> GetPlayersBy(int idTeam, List<int> idLanguages)
    {
        const string sql =
            @"SELECT sp.*, spn.name as ""name_name"", spn.fk_language as ""name_fk_language"", spn.version as ""name_version""" +
            " FROM lsquad_player sp " +
            " JOIN lsquad_player_name spn " +
            " ON spn.fk_player = sp.id_player " +
            " WHERE sp.fk_team = @fk_team " +
            " AND spn.fk_language = ANY (@fk_language) ";

        var parameters = new { fk_team = idTeam, fk_language = idLanguages };
        Console.WriteLine($"running {sql} with {parameters}");

        return GetConnection().Query<PlayerTransferWithName>(sql, parameters).ToList();
    }

    private List<PlayerTransfer> InsertIfNotExistsElseUpdateWithExternalId(List<PlayerTransfer> playerEntities)
    {
        List<string> valuesWithBrackets = [];
        List<int> uniqueList = [];
        foreach (PlayerTransfer playerTransfer in playerEntities)
        {
            if (uniqueList.Contains(playerTransfer.external_player_id)) { continue; }
            valuesWithBrackets.Add("(" + playerTransfer.external_player_id.ToString() + ", now())");
            uniqueList.Add(playerTransfer.external_player_id);
        }

        string sql =
            $@"INSERT INTO lsquad_player (external_player_id, created_at) VALUES {String.Join(", ", valuesWithBrackets)} " +
            "  ON CONFLICT (external_player_id) DO UPDATE SET updated_at = now() " +
            "  RETURNING *";

        Console.WriteLine($"running {sql} with {playerEntities.Count} playerEntities");

        return GetConnection().Query<PlayerTransfer>(sql).ToList();
    }
}


/*
1) current
SELECT sp.*, spn.name as ""name_name"", spn.fk_language as ""name_fk_language"", spn.version as ""name_version""
FROM lsquad_player sp
JOIN lsquad_player_name spn
ON spn.fk_player = sp.id_player
WHERE sp.fk_team = @fk_team
AND spn.fk_language = ANY (@fk_language)

2) if we land all versions in the same table
SELECT sp.*, spn.name as ""name_name"", spn.fk_language as ""name_fk_language"", spn.version as ""name_version""
FROM lsquad_player sp
JOIN lsquad_player_name spn ON spn.fk_player = sp.id_player
WHERE (sp.id_player, spn.version) IN (
    SELECT id_player, MAX(spn.version) AS max_version
    FROM lsquad_player sp
    JOIN lsquad_player_name spn ON spn.fk_player = sp.id_player
    WHERE sp.fk_team = @fk_team
    AND spn.fk_language = ANY (@fk_language)
    GROUP BY id_player
);
*/
