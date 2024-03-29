using Lsquad.Player.Transfer;

namespace Lsquad.Player.Persistence;

public interface IPlayerPersistence
{
    public void CreateOrUpdate(List<PlayerTransfer> playerEntities);

    public void CreateOrUpdateWithExternalId(List<PlayerTransfer> playerEntities);

    public List<PlayerTransferWithName> GetPlayersBy(int idTeam, List<int> idLanguages);
}
