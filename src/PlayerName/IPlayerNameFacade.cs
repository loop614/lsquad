using Lsquad.Player.Transfer;

namespace Lsquad.PlayerName;

public interface IPlayerNameFacade
{
    public void CreateOrUpdate(List<PlayerNameEntity> players);
}
