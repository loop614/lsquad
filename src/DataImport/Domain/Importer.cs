using Newtonsoft.Json;
using Lsquad.Setting;
using Lsquad.DataImport.Transfer;
using System.Diagnostics;

namespace Lsquad.DataImport.Domain;

public class Importer(IDispatcher dispatcher, ISettingService settingService) : IImporter
{
    public void ImportExample()
    {
        Stopwatch sw = new();
        sw.Start();
        var linesPlayer = File.ReadAllLines(@"./src/DataImport/ExampleData/br_domain_player.txt");
        for (var i = 0; i < linesPlayer.Length; i += 1)
        {
            BrDomainPlayer? brPlayer = JsonConvert.DeserializeObject<BrDomainPlayer>(linesPlayer[i]);
            if (brPlayer is null) { continue; }
            dispatcher.Dispatch(brPlayer);
        }

        var linesTeam = File.ReadAllLines(@"./src/DataImport/ExampleData/br_domain_team.txt");
        for (var i = 0; i < linesTeam.Length; i += 1)
        {
            BrDomainTeam? brTeam = JsonConvert.DeserializeObject<BrDomainTeam>(linesTeam[i]);
            if (brTeam is null) { continue; }
            dispatcher.Dispatch(brTeam);
        }

        var linesSquad = File.ReadAllLines(@"./src/DataImport/ExampleData/br_domain_squad.txt");
        for (var i = 0; i < linesSquad.Length; i += 1)
        {
            BrDomainSquad? brSquad = JsonConvert.DeserializeObject<BrDomainSquad>(linesSquad[i]);
            if (brSquad is null) { continue; }
            dispatcher.Dispatch(brSquad);
        }

        dispatcher.FlushCache("br_domain_squad");
        dispatcher.FlushCache("br_domain_player");
        dispatcher.FlushCache("br_domain_team");

        settingService.SetStatus("br_domain_player", SettingConfig.GetStatusDone());
        settingService.SetStatus("br_domain_team", SettingConfig.GetStatusDone());
        settingService.SetStatus("br_domain_squad", SettingConfig.GetStatusDone());
        sw.Stop();
        Console.WriteLine("ImportExample Elapsed={0}",sw.Elapsed);
    }
}
