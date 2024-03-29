using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Player.Transfer;

namespace Lsquad.PlayerName;

public class PlayerNamePersistence : LsquadPersistence, IPlayerNamePersistence
{
    public void CreateOrUpdate(List<PlayerNameTransfer> playerNames)
    {
        const string sql =
            @"INSERT INTO lsquad_player_name(fk_language, fk_player, version, name, created_at) VALUES (@fk_language, @fk_player, @version, @name, now()) " +
            " ON CONFLICT (fk_player, fk_language) DO UPDATE " +
            " set name = @name, version = @version, updated_at = now() ";
        Console.WriteLine($"running {sql} with {playerNames.Count} playerNameTransfers");

        GetConnection().Execute(sql, playerNames);
    }
}
