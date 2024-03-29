using Lsquad.Squad.Domain;
using Lsquad.Squad.Transfer;

namespace Lsquad.Squad;

public class SquadService(ISquadReader squadReader) : ISquadService
{
    public SquadResponse GetSquad(int externalTeamId, string lang)
    {
        return squadReader.GetSquad(externalTeamId, lang);
    }
}
