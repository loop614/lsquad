using Lsquad.Player.Transfer;

namespace Lsquad.PlayerName;

public class PlayerNameFacade : IPlayerNameFacade
{
    private readonly PlayerNameFactory _factory;

    public PlayerNameFacade()
    {
        _factory = new PlayerNameFactory();
    }

    public void CreateOrUpdate(List<PlayerNameEntity> players)
    {
        _factory.CreatePlayerNamePersistence().CreateOrUpdate(players);
    }
}
