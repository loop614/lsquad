using Lsquad.Squad.Transfer;

namespace Lsquad.Squad.Domain;

public interface ISquadReader
{
    public SquadResponse GetSquad(int external_team_id, string lang);
}
