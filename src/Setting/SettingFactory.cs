using Lsquad.Setting.Persistence;

namespace Lsquad.Setting;

public class SettingFactory
{
    public ISettingPersistence CreateSettingPersistence()
    {
        return new SettingPersistence();
    }
}
