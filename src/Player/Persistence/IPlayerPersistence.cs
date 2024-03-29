using Lsquad.Player.Transfer;

namespace Lsquad.Player.Persistence;

public interface IPlayerPersistence
{
    public void CreateOrUpdate(List<PlayerTransfer> playerTransfers);

    public void CreateOrUpdateWithExternalId(List<PlayerTransfer> playerTransfers);

    public List<PlayerTransferWithName> GetPlayersBy(int idTeam, List<int> idLanguages);
}
