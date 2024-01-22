using Lsquad.DataImport.ModuleDispatcher;
using Lsquad.Language;
using Lsquad.Player;
using Lsquad.Setting;
using Lsquad.Team;

namespace Lsquad.DataImport;

public class DataImportFactory
{
    public IImporter CreateImporter()
    {
        return new Importer(CreateDispatcher(), CreateSettingFacade());
    }

    private ISettingFacade CreateSettingFacade()
    {
        return new SettingFacade();
    }

    private IDispatcher CreateDispatcher()
    {
        return new Dispatcher(
            CreateLanguageFacade(),
            CreateTeamFacade(),
            CreatePlayerFacade()
        );
    }

    private IPlayerFacade CreatePlayerFacade()
    {
        return new PlayerFacade();
    }

    private ITeamFacade CreateTeamFacade()
    {
        return new TeamFacade();
    }

    private ILanguageFacade CreateLanguageFacade()
    {
        return new LanguageFacade();
    }
}
