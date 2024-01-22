using Lsquad.Language;
using Lsquad.Player;
using Lsquad.Player.Transfer;
using Lsquad.PlayerName;
using Lsquad.Squad.Transfer;
using Lsquad.Team;
using Lsquad.Team.Transfer;
using Lsquad.TeamName;

namespace Lsquad.Squad.Domain;

public class SquadReader : ISquadReader
{
    private readonly ILanguageFacade _languageFacade;

    private readonly ITeamFacade _teamFacade;

    private readonly ITeamNameFacade _teamNameFacade;

    private readonly IPlayerFacade _playerFacade;

    private readonly IPlayerNameFacade _playerNameFacade;

    public SquadReader(
        ILanguageFacade languageFacade,
        ITeamFacade teamFacade,
        ITeamNameFacade teamNameFacade,
        IPlayerFacade playerFacade,
        IPlayerNameFacade playerNameFacade
    ) {
        _languageFacade = languageFacade;
        _teamFacade = teamFacade;
        _teamNameFacade = teamNameFacade;
        _playerFacade = playerFacade;
        _playerNameFacade = playerNameFacade;
    }

    public SquadResponse GetSquad(int external_team_id, string lang)
    {
        SquadResponse squadResponse = new();
        Dictionary<string, int> langNameToId = _languageFacade.GetLanguagesNameToId([lang, "en"]);
        List<TeamIdName> teamIdNames = _teamFacade.GetTeamIdNameBy(external_team_id, langNameToId.Values.ToList());
        if (teamIdNames.Count == 0) {
            squadResponse.errors.Add("Team could not be found");
            return squadResponse;
        }
        squadResponse.team = new() { id = external_team_id, name = GetTeamName(teamIdNames, langNameToId, lang)};

        List<PlayerEntityWithName> playerEntities = _playerFacade.GetPlayersBy(teamIdNames[0].id_team, langNameToId.Values.ToList());
        Dictionary<int, Dictionary<int, PlayerEntityWithName>> idToPlayerEntities = [];
        foreach(PlayerEntityWithName pewn in playerEntities) {
            if (!idToPlayerEntities.ContainsKey(pewn.id_player)) {
                idToPlayerEntities[pewn.id_player] = [];
            }

            idToPlayerEntities[pewn.id_player][pewn.name_fk_language] = pewn;
        }

        squadResponse.players = TakePlayerEntities(idToPlayerEntities, langNameToId, lang);

        return squadResponse;
    }

    private string GetTeamName(List<TeamIdName> teamIdNames, Dictionary<string, int> langNameToId, string lang)
    {
        Dictionary<int, string> languageToTeamName = [];
        foreach(TeamIdName teamIdName in teamIdNames) {
            languageToTeamName[teamIdName.fk_language] = teamIdName.name;
        }
        if (langNameToId.ContainsKey(lang) && languageToTeamName.TryGetValue(langNameToId[lang], out string? value)) {
            return value;
        }

        return languageToTeamName[langNameToId["en"]];
    }

    private List<SquadResponsePlayer> TakePlayerEntities(
        Dictionary<int, Dictionary<int, PlayerEntityWithName>> idToPlayerEntities,
        Dictionary<string, int> langNameToId,
        string lang
    ) {
        List<SquadResponsePlayer> srpl = [];
        foreach(Dictionary<int, PlayerEntityWithName> fkLangToPlayerEntity in idToPlayerEntities.Values) {
            PlayerEntityWithName usedPlayerEntity;
            if (langNameToId.ContainsKey(lang) && fkLangToPlayerEntity.TryGetValue(langNameToId[lang], out PlayerEntityWithName? value)) {
                usedPlayerEntity = value;
            } else {
                usedPlayerEntity = fkLangToPlayerEntity[langNameToId["en"]];
            }

            SquadResponsePlayer srp = new()
            {
                fullName = usedPlayerEntity.name_name ?? usedPlayerEntity.full_name,
                player_id = usedPlayerEntity.external_player_id,
                weight = usedPlayerEntity.weight,
                shirt_number = usedPlayerEntity.shirt_number,
                preferred_foot = usedPlayerEntity.preferred_foot,
                position = usedPlayerEntity.position,
                last_name = usedPlayerEntity.last_name,
                height = usedPlayerEntity.height,
                first_name = usedPlayerEntity.first_name,
                country_code = usedPlayerEntity.country_code,
                birth_date = usedPlayerEntity.birth_date
            };
            srpl.Add(srp);
        }

        return srpl;
    }
}
