namespace Lsquad.Setting;

public class SettingFacade : ISettingFacade
{
    private readonly SettingFactory _factory;

    public SettingFacade()
    {
        _factory = new SettingFactory();
    }

    public int GetStatusForSetting(string settingName)
    {
        return _factory.CreateSettingPersistence().GetStatusForSetting(settingName);
    }

    public bool AreAllStatusesReady()
    {
        return _factory.CreateSettingPersistence().AreAllStatusesReady();
    }

    public void SetStatus(string settingName, int status)
    {
        _factory.CreateSettingPersistence().SetStatus(settingName, status);
    }
}
