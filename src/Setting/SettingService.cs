using Lsquad.Setting.Persistence;

namespace Lsquad.Setting;

public class SettingService(ISettingPersistence settingPersistence) : ISettingService
{
    public int? GetStatusForSetting(string settingName)
    {
        return settingPersistence.GetStatusForSetting(settingName);
    }

    public bool AreAllStatusesReady()
    {
        return settingPersistence.AreAllStatusesReady();
    }

    public void SetStatus(string settingName, int status)
    {
        settingPersistence.SetStatus(settingName, status);
    }
}
