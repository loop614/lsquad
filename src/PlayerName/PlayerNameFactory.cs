namespace Lsquad.PlayerName;

public class PlayerNameFactory
{
    public IPlayerNamePersistence CreatePlayerNamePersistence()
    {
        return new PlayerNamePersistence();
    }
}
