using Lsquad.Squad.Transfer;

namespace Lsquad.Squad;

public class SquadFacade : ISquadFacade
{
    private readonly SquadFactory _factory = new SquadFactory();
    public SquadResponse GetSquad(int external_team_id, string lang)
    {
        return _factory.CreateSquadReader().GetSquad(external_team_id, lang);
    }
}
