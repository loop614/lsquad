using Lsquad.DataImport.Transfer;
using Lsquad.Language;
using Lsquad.Player;
using Lsquad.Player.Transfer;
using Lsquad.Team;
using Lsquad.Team.Transfer;
using Lsquad.TeamName.Transfer;

namespace Lsquad.DataImport.Domain;

public class Dispatcher(
    ILanguageService languageService,
    ITeamService teamService,
    IPlayerService playerService
) : IDispatcher
{
    private static List<PlayerTransfer> playerTransfersCache = [];

    private static List<PlayerTransfer> playerTransfersWithExternalIdCache = [];

    private static List<TeamTransfer> teamTransfersWithExternalIdCache = [];

    public void Dispatch(BrDomainPlayer brDomainPlayer)
    {
        List<string> langs = [];
        if (brDomainPlayer.names is null) { return; }
        double version = brDomainPlayer.version;
        foreach (BrDomainPlayerNameStat stat in brDomainPlayer.names.stats)
        {
            if (stat.lang is not null)
            {
                langs.Add(stat.lang);
            }
        }
        foreach (BrDomainPlayerNameLiveScore livescore in brDomainPlayer.names.livescore)
        {
            if (livescore.lang is not null)
            {
                langs.Add(livescore.lang);
            }
        }
        Dictionary<string, int> languageNameToId = languageService.GetOrCreate(langs);
        PlayerTransfer playerTransfer = new() { external_player_id = brDomainPlayer.id };

        foreach (BrDomainPlayerNameStat stat in brDomainPlayer.names.stats)
        {
            if (stat.lang is null || stat.name is null || !languageNameToId.ContainsKey(stat.lang))
            {
                continue;
            }
            var playerNameTransfer = new PlayerNameTransfer
            {
                name = stat.name,
                fk_language = languageNameToId[stat.lang],
                version = version
            };
            playerTransfer.playerNameTransfers.Add(playerNameTransfer);
        }

        foreach (BrDomainPlayerNameLiveScore livescore in brDomainPlayer.names.livescore)
        {
            if (livescore.lang is null || livescore.name is null || !languageNameToId.ContainsKey(livescore.lang))
            {
                continue;
            }
            var playerNameTransfer = new PlayerNameTransfer
            {
                name = livescore.name,
                fk_language = languageNameToId[livescore.lang],
                version = version
            };
            playerTransfer.playerNameTransfers.Add(playerNameTransfer);
        }

        playerTransfersWithExternalIdCache.Add(playerTransfer);
        if (playerTransfersWithExternalIdCache.Count > 100)
        {
            FlushCache("br_domain_player");
        }
    }

    public void Dispatch(BrDomainSquad brDomainSquad)
    {
        TeamTransfer teamTransfer = new() { external_team_id = brDomainSquad.team_id };
        teamTransfer = teamService.CreateOrUpdateByExternalTeamId(teamTransfer);
        List<PlayerTransfer> playerTransfers = [];
        foreach (BrDomainSquadPlayer player in brDomainSquad.players)
        {
            if (player.player_id is null) continue;
            DateTime? birthDateParsed = null;
            if (DateTime.TryParse(player.birth_date, out DateTime birthDate))
            {
                birthDateParsed = birthDate;
            }
            var playerTransfer = new PlayerTransfer
            {
                fk_team = teamTransfer.id_team,
                version = brDomainSquad.version,
                weight = player.weight,
                shirt_number = player.shirt_number,
                preferred_foot = player.preferred_foot,
                position = player.position,
                external_player_id = (int)player.player_id,
                last_name = player.last_name,
                height = player.height,
                full_name = player.full_name,
                first_name = player.first_name,
                country_code = player.country_code,
                birth_date = birthDateParsed,
            };

            playerTransfers.Add(playerTransfer);
        }

        playerTransfersCache.AddRange(playerTransfers);
        if (playerTransfersCache.Count > 200)
        {
            FlushCache("br_domain_squad");
        }
    }

    public void Dispatch(BrDomainTeam brDomainTeam)
    {
        List<string> langs = [];
        if (brDomainTeam.names is null) { return; }
        double version = brDomainTeam.version;
        foreach (BrDomainTeamNameStat stat in brDomainTeam.names.stats)
        {
            if (stat.lang is not null)
            {
                langs.Add(stat.lang);
            }
        }
        foreach (BrDomainTeamNameLivescore stat in brDomainTeam.names.livescore)
        {
            if (stat.lang is not null)
            {
                langs.Add(stat.lang);
            }
        }
        Dictionary<string, int> languageNameToId = languageService.GetOrCreate(langs);
        TeamTransfer teamTransfer = new() { external_team_id = brDomainTeam.id };

        foreach (BrDomainTeamNameStat stat in brDomainTeam.names.stats)
        {
            if (stat.lang is null || stat.name is null || !languageNameToId.ContainsKey(stat.lang))
            {
                continue;
            }
            var teamNameTransfer = new TeamNameTransfer
            {
                name = stat.name,
                fk_language = languageNameToId[stat.lang],
                version = version
            };
            teamTransfer.AddTeamNameTransfer(teamNameTransfer);
        }
        foreach (BrDomainTeamNameLivescore livescore in brDomainTeam.names.livescore)
        {
            if (livescore.lang is null || livescore.name is null || !languageNameToId.ContainsKey(livescore.lang))
            {
                continue;
            }
            var teamNameTransfer = new TeamNameTransfer
            {
                name = livescore.name,
                fk_language = languageNameToId[livescore.lang],
                version = version
            };
            teamTransfer.AddTeamNameTransfer(teamNameTransfer);
        }

        teamTransfersWithExternalIdCache.Add(teamTransfer);
        if (teamTransfersWithExternalIdCache.Count > 100)
        {
            FlushCache("br_domain_team");
        }
    }

    public void FlushCache(string topic)
    {
        switch (topic)
        {
            case "br_domain_squad":
                playerService.CreateOrUpdate(playerTransfersCache);
                playerTransfersCache = [];
                break;

            case "br_domain_player":
                playerService.CreateOrUpdateWithExternalId(playerTransfersWithExternalIdCache);
                playerTransfersWithExternalIdCache = [];
                break;

            case "br_domain_team":
                teamService.CreateOrUpdateByExternalTeamId(teamTransfersWithExternalIdCache);
                teamTransfersWithExternalIdCache = [];
                break;

            default:
                return;
        }
    }
}
