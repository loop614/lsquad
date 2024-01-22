using Newtonsoft.Json;
using Lsquad.Setting;
using Lsquad.DataImport.ModuleDispatcher;
using Lsquad.DataImport.Transfer;

namespace Lsquad.DataImport;

public class Importer : IImporter
{
    private readonly IDispatcher _dispatcher;

    private readonly ISettingFacade _settingFacade;

    public Importer(
        IDispatcher dispatcher,
        ISettingFacade settingFacade
    ) {
        _dispatcher = dispatcher;
        _settingFacade = settingFacade;
    }

    public void ImportExample()
    {
        List<BrDomainPlayer> brDomainPlayers = [];
        List<BrDomainSquad> brDomainSquads = [];
        List<BrDomainTeam> brDomainTeams = [];

        var linesPlayer = File.ReadAllLines(@"./src/DataImport/Example/br_domain_player.txt");
        for (var i = 0; i < linesPlayer.Length; i += 1) {
            BrDomainPlayer brPlayer = JsonConvert.DeserializeObject<BrDomainPlayer>(linesPlayer[i]);
            brDomainPlayers.Add(brPlayer);
        }

        var linesTeam = File.ReadAllLines(@"./src/DataImport/Example/br_domain_team.txt");
        for (var i = 0; i < linesTeam.Length; i += 1) {
            BrDomainTeam brTeam = JsonConvert.DeserializeObject<BrDomainTeam>(linesTeam[i]);
            brDomainTeams.Add(brTeam);
        }

        var linesSquad = File.ReadAllLines(@"./src/DataImport/Example/br_domain_squad.txt");
        for (var i = 0; i < linesSquad.Length; i += 1) {
            BrDomainSquad brSquad = JsonConvert.DeserializeObject<BrDomainSquad>(linesSquad[i]);
            brDomainSquads.Add(brSquad);
        }

        foreach (var brDomainPlayer in brDomainPlayers)
        {
            _dispatcher.Dispatch(brDomainPlayer);
        }

        foreach (var brDomainTeam in brDomainTeams)
        {
            _dispatcher.Dispatch(brDomainTeam);
        }

        foreach (var brDomainSquad in brDomainSquads)
        {
            _dispatcher.Dispatch(brDomainSquad);
        }

        _dispatcher.FlushCache("br_domain_squad");
        _dispatcher.FlushCache("br_domain_player");
        _dispatcher.FlushCache("br_domain_team");

        _settingFacade.SetStatus("br_domain_player", 1);
        _settingFacade.SetStatus("br_domain_team", 1);
        _settingFacade.SetStatus("br_domain_squad", 1);
    }
}
