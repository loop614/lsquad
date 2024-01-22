using Lsquad.Squad.Transfer;

namespace Lsquad.Squad;

public interface ISquadFacade
{
    public SquadResponse GetSquad(int external_team_id, string lang);
}
