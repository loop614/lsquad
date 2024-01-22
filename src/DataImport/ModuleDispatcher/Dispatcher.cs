using Lsquad.DataImport.Transfer;
using Lsquad.Language;
using Lsquad.Player;
using Lsquad.Player.Transfer;
using Lsquad.Team;
using Lsquad.Team.Transfer;
using Lsquad.TeamName.Transfer;

namespace Lsquad.DataImport.ModuleDispatcher;

public class Dispatcher : IDispatcher
{
    private readonly ILanguageFacade _languageFacade;

    private readonly ITeamFacade _teamFacade;

    private readonly IPlayerFacade _playerFacade;

    private static List<PlayerEntity> _playerEntitiesCache = [];

    private static List<PlayerEntity> _playerEntitiesWithExternalIdCache = [];

    private static List<TeamEntity> _teamEntitiesWithExternalIdCache = [];

    public Dispatcher(
        ILanguageFacade languageFacade,
        ITeamFacade teamFacade,
        IPlayerFacade playerFacade
    ) {
        _languageFacade = languageFacade;
        _teamFacade = teamFacade;
        _playerFacade = playerFacade;
    }

    public void Dispatch(BrDomainPlayer brDomainPlayer)
    {
        List<string> langs = new();
        if (brDomainPlayer.names is null) {return;}
        double version = brDomainPlayer.version;
        foreach(BrDomainPlayerNameStat stat in brDomainPlayer.names.stats) {
            if (stat.lang is not null) {
                langs.Add(stat.lang);
            }
        }
        foreach(BrDomainPlayerNameLiveScore livescore in brDomainPlayer.names.livescore) {
            if (livescore.lang is not null) {
                langs.Add(livescore.lang);
            }
        }
        Dictionary<string, int> languageNameToId = _languageFacade.GetOrCreate(langs);
        PlayerEntity playerEntity = new() { external_player_id = brDomainPlayer.id };

        foreach (BrDomainPlayerNameStat stat in brDomainPlayer.names.stats) {
            if (stat.lang is null || stat.name is null || !languageNameToId.ContainsKey(stat.lang)) {
                continue;
            }
            var playerNameEntity = new PlayerNameEntity
            {
                name = stat.name,
                fk_language = languageNameToId[stat.lang],
                version = version
            };
            playerEntity.playerNameEntities.Add(playerNameEntity);
        }

        foreach(BrDomainPlayerNameLiveScore livescore in brDomainPlayer.names.livescore) {
            if (livescore.lang is null || livescore.name is null || !languageNameToId.ContainsKey(livescore.lang)) {
                continue;
            }
            var playerNameEntity = new PlayerNameEntity
            {
                name = livescore.name,
                fk_language = languageNameToId[livescore.lang],
                version = version
            };
            playerEntity.playerNameEntities.Add(playerNameEntity);
        }

        _playerEntitiesWithExternalIdCache.Add(playerEntity);
        if (_playerEntitiesWithExternalIdCache.Count > 100) {
            FlushCache("br_domain_player");
        }
    }

    public void Dispatch(BrDomainSquad brDomainSquad)
    {
        TeamEntity teamEntity = new() { external_team_id = brDomainSquad.team_id};
        teamEntity = _teamFacade.CreateOrUpdateByExternalTeamId(teamEntity);
        List<PlayerEntity> playerEntities = [];
        foreach(BrDomainSquadPlayer player in brDomainSquad.players) {
            if (player.player_id is null) continue;
            DateTime? birthDateParsed = null;
            if (DateTime.TryParse(player.birth_date, out DateTime birthDate)) {
                birthDateParsed = birthDate;
            }
            var playerEntity = new PlayerEntity
            {
                fk_team = teamEntity.id_team,
                version = brDomainSquad.version,
                weight = player.weight,
                shirt_number = player.shirt_number,
                preferred_foot = player.preferred_foot,
                position = player.position,
                external_player_id = (int) player.player_id,
                last_name = player.last_name,
                height = player.height,
                full_name = player.full_name,
                first_name = player.first_name,
                country_code = player.country_code,
                birth_date = birthDateParsed,
            };

            playerEntities.Add(playerEntity);
        }

        _playerEntitiesCache.AddRange(playerEntities);
        if (_playerEntitiesCache.Count > 200) {
            FlushCache("br_domain_squad");
        }
    }

    public void Dispatch(BrDomainTeam brDomainTeam)
    {
        List<string> langs = new();
        if (brDomainTeam.names is null) {return;}
        double version = brDomainTeam.version;
        foreach(BrDomainTeamNameStat stat in brDomainTeam.names.stats) {
            if (stat.lang is not null) {
                langs.Add(stat.lang);
            }
        }
        foreach(BrDomainTeamNameLivescore stat in brDomainTeam.names.livescore) {
            if (stat.lang is not null) {
                langs.Add(stat.lang);
            }
        }
        Dictionary<string, int> languageNameToId = _languageFacade.GetOrCreate(langs);
        TeamEntity teamEntity = new() { external_team_id = brDomainTeam.id };

        foreach (BrDomainTeamNameStat stat in brDomainTeam.names.stats) {
            if (stat.lang is null || stat.name is null || !languageNameToId.ContainsKey(stat.lang)) {
                continue;
            }
            var teamNameEntity = new TeamNameEntity
            {
                name = stat.name,
                fk_language = languageNameToId[stat.lang],
                version = version
            };
            teamEntity.AddTeamNameEntity(teamNameEntity);
        }
        foreach (BrDomainTeamNameLivescore livescore in brDomainTeam.names.livescore) {
            if (livescore.lang is null || livescore.name is null || !languageNameToId.ContainsKey(livescore.lang)) {
                continue;
            }
            var teamNameEntity = new TeamNameEntity
            {
                name = livescore.name,
                fk_language = languageNameToId[livescore.lang],
                version = version
            };
            teamEntity.AddTeamNameEntity(teamNameEntity);
        }

        _teamEntitiesWithExternalIdCache.Add(teamEntity);
        if (_teamEntitiesWithExternalIdCache.Count > 100) {
            FlushCache("br_domain_team");
        }
    }

    public void FlushCache(string topic)
    {
        switch (topic)
        {
            case "br_domain_squad":
                _playerFacade.CreateOrUpdate(_playerEntitiesCache);
                _playerEntitiesCache = [];
                break;

            case "br_domain_player":
                _playerFacade.CreateOrUpdateWithExternalId(_playerEntitiesWithExternalIdCache);
                _playerEntitiesWithExternalIdCache = [];
                break;

            case "br_domain_team":
                _teamFacade.CreateOrUpdateByExternalTeamId(_teamEntitiesWithExternalIdCache);
                _teamEntitiesWithExternalIdCache = [];
                break;

            default:
                return;
        }
    }
}
