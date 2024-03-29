using Lsquad.Player.Transfer;

namespace Lsquad.Player;

public interface IPlayerService
{
    public void CreateOrUpdate(List<PlayerTransfer> playerEntities);

    public void CreateOrUpdateWithExternalId(List<PlayerTransfer> playerEntities);

    public List<PlayerTransferWithName> GetPlayersBy(int idTeam, List<int> idLanguages);
}
