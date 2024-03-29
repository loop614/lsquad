using Lsquad.Player.Transfer;

namespace Lsquad.PlayerName;

public class PlayerNameService(IPlayerNamePersistence playerNamePersistence) : IPlayerNameService
{
    public void CreateOrUpdate(List<PlayerNameTransfer> players)
    {
        playerNamePersistence.CreateOrUpdate(players);
    }
}
