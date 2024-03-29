using Lsquad.Player.Persistence;
using Lsquad.Player.Transfer;

namespace Lsquad.Player;

public class PlayerService(IPlayerPersistence playerPersistence) : IPlayerService
{
    public void CreateOrUpdate(List<PlayerTransfer> playerTransfers)
    {
        playerPersistence.CreateOrUpdate(playerTransfers);
    }

    public void CreateOrUpdateWithExternalId(List<PlayerTransfer> playerTransfers)
    {
        playerPersistence.CreateOrUpdateWithExternalId(playerTransfers);
    }

    public List<PlayerTransferWithName> GetPlayersBy(int idTeam, List<int> idLanguages)
    {
        return playerPersistence.GetPlayersBy(idTeam, idLanguages);
    }
}
