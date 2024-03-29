using Lsquad.DataImport.Transfer;
using Lsquad.Language;
using Lsquad.Player;
using Lsquad.Player.Transfer;
using Lsquad.Team;
using Lsquad.Team.Transfer;
using Lsquad.TeamName.Transfer;

namespace Lsquad.DataImport.Domain;

public class Dispatcher : IDispatcher
{
    private readonly ILanguageService _languageService;

    private readonly ITeamService _teamService;

    private readonly IPlayerService _playerService;

    private static List<PlayerTransfer> _playerEntitiesCache = [];

    private static List<PlayerTransfer> _playerEntitiesWithExternalIdCache = [];

    private static List<TeamTransfer> _teamEntitiesWithExternalIdCache = [];

    public Dispatcher(
        ILanguageService languageService,
        ITeamService teamService,
        IPlayerService playerService
    ) {
        _languageService = languageService;
        _teamService = teamService;
        _playerService = playerService;
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
        Dictionary<string, int> languageNameToId = _languageService.GetOrCreate(langs);
        PlayerTransfer playerTransfer = new() { external_player_id = brDomainPlayer.id };

        foreach (BrDomainPlayerNameStat stat in brDomainPlayer.names.stats) {
            if (stat.lang is null || stat.name is null || !languageNameToId.ContainsKey(stat.lang)) {
                continue;
            }
            var playerNameEntity = new PlayerNameTransfer
            {
                name = stat.name,
                fk_language = languageNameToId[stat.lang],
                version = version
            };
            playerTransfer.playerNameEntities.Add(playerNameEntity);
        }

        foreach(BrDomainPlayerNameLiveScore livescore in brDomainPlayer.names.livescore) {
            if (livescore.lang is null || livescore.name is null || !languageNameToId.ContainsKey(livescore.lang)) {
                continue;
            }
            var playerNameEntity = new PlayerNameTransfer
            {
                name = livescore.name,
                fk_language = languageNameToId[livescore.lang],
                version = version
            };
            playerTransfer.playerNameEntities.Add(playerNameEntity);
        }

        _playerEntitiesWithExternalIdCache.Add(playerTransfer);
        if (_playerEntitiesWithExternalIdCache.Count > 100) {
            FlushCache("br_domain_player");
        }
    }

    public void Dispatch(BrDomainSquad brDomainSquad)
    {
        TeamTransfer teamEntity = new() { externalTeamId = brDomainSquad.team_id};
        teamEntity = _teamService.CreateOrUpdateByExternalTeamId(teamEntity);
        List<PlayerTransfer> playerEntities = [];
        foreach(BrDomainSquadPlayer player in brDomainSquad.players) {
            if (player.player_id is null) continue;
            DateTime? birthDateParsed = null;
            if (DateTime.TryParse(player.birth_date, out DateTime birthDate)) {
                birthDateParsed = birthDate;
            }
            var playerTransfer = new PlayerTransfer
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

            playerEntities.Add(playerTransfer);
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
        Dictionary<string, int> languageNameToId = _languageService.GetOrCreate(langs);
        TeamTransfer teamEntity = new() { externalTeamId = brDomainTeam.id };

        foreach (BrDomainTeamNameStat stat in brDomainTeam.names.stats) {
            if (stat.lang is null || stat.name is null || !languageNameToId.ContainsKey(stat.lang)) {
                continue;
            }
            var teamNameTransfer = new TeamNameTransfer
            {
                name = stat.name,
                fk_language = languageNameToId[stat.lang],
                version = version
            };
            teamEntity.AddTeamNameTransfer(teamNameTransfer);
        }
        foreach (BrDomainTeamNameLivescore livescore in brDomainTeam.names.livescore) {
            if (livescore.lang is null || livescore.name is null || !languageNameToId.ContainsKey(livescore.lang)) {
                continue;
            }
            var teamNameTransfer = new TeamNameTransfer
            {
                name = livescore.name,
                fk_language = languageNameToId[livescore.lang],
                version = version
            };
            teamEntity.AddTeamNameTransfer(teamNameTransfer);
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
                _playerService.CreateOrUpdate(_playerEntitiesCache);
                _playerEntitiesCache = [];
                break;

            case "br_domain_player":
                _playerService.CreateOrUpdateWithExternalId(_playerEntitiesWithExternalIdCache);
                _playerEntitiesWithExternalIdCache = [];
                break;

            case "br_domain_team":
                _teamService.CreateOrUpdateByExternalTeamId(_teamEntitiesWithExternalIdCache);
                _teamEntitiesWithExternalIdCache = [];
                break;

            default:
                return;
        }
    }
}
