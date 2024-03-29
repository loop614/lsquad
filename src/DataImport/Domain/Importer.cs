using Newtonsoft.Json;
using Lsquad.Setting;
using Lsquad.DataImport.Transfer;

namespace Lsquad.DataImport.Domain;

public class Importer(IDispatcher dispatcher, ISettingService settingService) : IImporter
{
    public void ImportExample()
    {
        List<BrDomainPlayer> brDomainPlayers = [];
        List<BrDomainSquad> brDomainSquads = [];
        List<BrDomainTeam> brDomainTeams = [];

        var linesPlayer = File.ReadAllLines(@"./src/DataImport/ExampleData/br_domain_player.txt");
        for (var i = 0; i < linesPlayer.Length; i += 1)
        {
            BrDomainPlayer? brPlayer = JsonConvert.DeserializeObject<BrDomainPlayer>(linesPlayer[i]);
            if (brPlayer is null) { continue; }
            brDomainPlayers.Add(brPlayer);
        }

        var linesTeam = File.ReadAllLines(@"./src/DataImport/ExampleData/br_domain_team.txt");
        for (var i = 0; i < linesTeam.Length; i += 1)
        {
            BrDomainTeam? brTeam = JsonConvert.DeserializeObject<BrDomainTeam>(linesTeam[i]);
            if (brTeam is null) { continue; }
            brDomainTeams.Add(brTeam);
        }

        var linesSquad = File.ReadAllLines(@"./src/DataImport/ExampleData/br_domain_squad.txt");
        for (var i = 0; i < linesSquad.Length; i += 1)
        {
            BrDomainSquad? brSquad = JsonConvert.DeserializeObject<BrDomainSquad>(linesSquad[i]);
            if (brSquad is null) { continue; }
            brDomainSquads.Add(brSquad);
        }

        foreach (var brDomainPlayer in brDomainPlayers)
        {
            dispatcher.Dispatch(brDomainPlayer);
        }

        foreach (var brDomainTeam in brDomainTeams)
        {
            dispatcher.Dispatch(brDomainTeam);
        }

        foreach (var brDomainSquad in brDomainSquads)
        {
            dispatcher.Dispatch(brDomainSquad);
        }

        dispatcher.FlushCache("br_domain_squad");
        dispatcher.FlushCache("br_domain_player");
        dispatcher.FlushCache("br_domain_team");

        settingService.SetStatus("br_domain_player", SettingConfig.GetStatusDone());
        settingService.SetStatus("br_domain_team", SettingConfig.GetStatusDone());
        settingService.SetStatus("br_domain_squad", SettingConfig.GetStatusDone());
    }
}
