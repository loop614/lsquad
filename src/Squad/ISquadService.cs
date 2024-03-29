using Lsquad.Squad.Transfer;

namespace Lsquad.Squad;

public interface ISquadService
{
    public SquadResponse GetSquad(int externalTeamId, string lang);
}
