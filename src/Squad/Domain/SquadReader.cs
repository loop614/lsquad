using Lsquad.Language;
using Lsquad.Player;
using Lsquad.Player.Transfer;
using Lsquad.Squad.Transfer;
using Lsquad.Team;
using Lsquad.Team.Transfer;

namespace Lsquad.Squad.Domain;

public class SquadReader(
    ILanguageService languageService,
    ITeamService teamService,
    IPlayerService playerService
) : ISquadReader
{
    public SquadResponse GetSquad(int externalTeamId, string lang)
    {
        SquadResponse squadResponse = new();
        Dictionary<string, int> langNameToId = languageService.GetLanguagesNameToId([lang, "en"]);
        List<TeamIdName> teamIdNames = teamService.GetTeamIdNameBy(externalTeamId, langNameToId.Values.ToList());
        if (teamIdNames.Count == 0)
        {
            squadResponse.errors.Add("Team could not be found");
            return squadResponse;
        }
        squadResponse.team = new() { id = externalTeamId, name = GetTeamName(teamIdNames, langNameToId, lang) };

        List<PlayerTransferWithName> playerTransfers = playerService.GetPlayersBy(teamIdNames[0].id_team, langNameToId.Values.ToList());
        Dictionary<int, Dictionary<int, PlayerTransferWithName>> idToPlayerTransfers = new();
        foreach (PlayerTransferWithName pewn in playerTransfers)
        {
            if (!idToPlayerTransfers.ContainsKey(pewn.id_player))
            {
                idToPlayerTransfers[pewn.id_player] = [];
            }

            idToPlayerTransfers[pewn.id_player][pewn.name_fk_language] = pewn;
        }

        squadResponse.players = TakePlayerTransfers(idToPlayerTransfers, langNameToId, lang);

        return squadResponse;
    }

    private string GetTeamName(List<TeamIdName> teamIdNames, Dictionary<string, int> langNameToId, string lang)
    {
        Dictionary<int, string> languageToTeamName = new();
        foreach (TeamIdName teamIdName in teamIdNames)
        {
            languageToTeamName[teamIdName.fk_language] = teamIdName.name!;
        }

        if (langNameToId.ContainsKey(lang) && languageToTeamName.TryGetValue(langNameToId[lang], out string? value))
        {
            return value;
        }

        return languageToTeamName[langNameToId["en"]];
    }

    private List<SquadResponsePlayer> TakePlayerTransfers(
        Dictionary<int, Dictionary<int, PlayerTransferWithName>> idToPlayerTransfers,
        Dictionary<string, int> langNameToId,
        string lang
    )
    {
        List<SquadResponsePlayer> srpl = [];
        foreach (Dictionary<int, PlayerTransferWithName> fkLangToPlayerTransfer in idToPlayerTransfers.Values)
        {
            PlayerTransferWithName usedPlayerTransfer;
            if (langNameToId.ContainsKey(lang) && fkLangToPlayerTransfer.TryGetValue(langNameToId[lang], out PlayerTransferWithName? value))
            {
                usedPlayerTransfer = value;
            }
            else
            {
                usedPlayerTransfer = fkLangToPlayerTransfer[langNameToId["en"]];
            }

            SquadResponsePlayer srp = new()
            {
                fullName = usedPlayerTransfer.name_name ?? usedPlayerTransfer.full_name,
                player_id = usedPlayerTransfer.external_player_id,
                weight = usedPlayerTransfer.weight,
                shirt_number = usedPlayerTransfer.shirt_number,
                preferred_foot = usedPlayerTransfer.preferred_foot,
                position = usedPlayerTransfer.position,
                last_name = usedPlayerTransfer.last_name,
                height = usedPlayerTransfer.height,
                first_name = usedPlayerTransfer.first_name,
                country_code = usedPlayerTransfer.country_code,
                birth_date = usedPlayerTransfer.birth_date
            };
            srpl.Add(srp);
        }

        return srpl;
    }
}
