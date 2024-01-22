using Microsoft.AspNetCore.Mvc;
using Lsquad.Setting;
using Lsquad.Squad.Transfer;

namespace Lsquad.Squad.Controller;

[Route("api/squad/")]
[ApiController]
public class SquadController : ControllerBase
{
    private readonly ISquadFacade _facade;

    private readonly ISettingFacade _settingFacade;

    public SquadController()
    {
        _facade = new SquadFacade();
        _settingFacade = new SettingFacade();
    }

    [Route("{external_team_id}/{lang}")]
    public SquadResponse Squad(int external_team_id, string? lang)
    {
        lang ??= "en";
        if(_settingFacade.AreAllStatusesReady()) {
            return _facade.GetSquad(external_team_id, lang);
        }

        var res = new SquadResponse();
        res.errors.Add("Data Import still in progress");

        return res;
    }
}
