using Lsquad.Player.Transfer;

namespace Lsquad.Player;

public class PlayerFacade : IPlayerFacade
{
    private readonly PlayerFactory _factory;

    public PlayerFacade()
    {
        _factory = new PlayerFactory();
    }

    public void CreateOrUpdate(List<PlayerEntity> playerEntities)
    {
        _factory.CreatePlayerPersistence().CreateOrUpdate(playerEntities);
    }

    public void CreateOrUpdateWithExternalId(List<PlayerEntity> playerEntities)
    {
        _factory.CreatePlayerPersistence().CreateOrUpdateWithExternalId(playerEntities);
    }

    public List<PlayerEntityWithName> GetPlayersBy(int idTeam, List<int> idLanguages)
    {
        return _factory.CreatePlayerPersistence().GetPlayersBy(idTeam, idLanguages);
    }
}
