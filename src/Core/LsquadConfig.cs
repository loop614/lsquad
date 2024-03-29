using Lsquad.DataImport;
using Lsquad.Language;
using Lsquad.Player;
using Lsquad.PlayerName;
using Lsquad.Setting;
using Lsquad.Squad;
using Lsquad.Team;
using Lsquad.TeamName;

namespace Lsquad.Core;

public static class LsquadConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        SettingConfig.AddBuilderServices(builder);
        LanguageConfig.AddBuilderServices(builder);
        PlayerNameConfig.AddBuilderServices(builder);
        PlayerConfig.AddBuilderServices(builder);
        TeamNameConfig.AddBuilderServices(builder);
        TeamConfig.AddBuilderServices(builder);
        DataImportConfig.AddBuilderServices(builder);
        SquadConfig.AddBuilderServices(builder);
    }
}
