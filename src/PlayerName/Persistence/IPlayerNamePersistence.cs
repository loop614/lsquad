using Lsquad.Player.Transfer;

namespace Lsquad.PlayerName;

public interface IPlayerNamePersistence
{
    public void CreateOrUpdate(List<PlayerNameEntity> players);
}
