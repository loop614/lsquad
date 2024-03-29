using Lsquad.Player.Persistence;
using Lsquad.Player.Transfer;

namespace Lsquad.Player;

public class PlayerService(IPlayerPersistence playerPersistence) : IPlayerService
{
    public void CreateOrUpdate(List<PlayerTransfer> playerEntities)
    {
        playerPersistence.CreateOrUpdate(playerEntities);
    }

    public void CreateOrUpdateWithExternalId(List<PlayerTransfer> playerEntities)
    {
        playerPersistence.CreateOrUpdateWithExternalId(playerEntities);
    }

    public List<PlayerTransferWithName> GetPlayersBy(int idTeam, List<int> idLanguages)
    {
        return playerPersistence.GetPlayersBy(idTeam, idLanguages);
    }
}
