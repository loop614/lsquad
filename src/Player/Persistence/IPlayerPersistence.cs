using Lsquad.Player.Transfer;

namespace Lsquad.Player.Persistence;

public interface IPlayerPersistence
{
    public void CreateOrUpdate(List<PlayerEntity> playerEntities);

    public void CreateOrUpdateWithExternalId(List<PlayerEntity> playerEntities);

    public List<PlayerEntityWithName> GetPlayersBy(int idTeam, List<int> idLanguages);
}
