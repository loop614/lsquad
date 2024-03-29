using Lsquad.Player.Transfer;

namespace Lsquad.PlayerName;

public interface IPlayerNameService
{
    public void CreateOrUpdate(List<PlayerNameTransfer> players);
}
