using Lsquad.Player.Persistence;

namespace Lsquad.Player;

public class PlayerFactory
{
    public IPlayerPersistence CreatePlayerPersistence()
    {
        return new PlayerPersistence();
    }
}
