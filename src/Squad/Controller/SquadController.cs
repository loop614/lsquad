using Microsoft.AspNetCore.Mvc;
using Lsquad.Setting;
using Lsquad.Squad.Transfer;

namespace Lsquad.Squad.Controller;

[Route("api/squad/")]
[ApiController]
public class SquadController(ISquadService squadService, ISettingService settingService) : ControllerBase
{
    [Route("{externalTeamId}/{lang}")]
    public SquadResponse Squad(int externalTeamId, string? lang)
    {
        lang ??= "en";
        if(settingService.AreAllStatusesReady()) {
            return squadService.GetSquad(externalTeamId, lang);
        }

        var res = new SquadResponse();
        res.errors.Add("Data Import still in progress. Please wait...");

        return res;
    }
}
