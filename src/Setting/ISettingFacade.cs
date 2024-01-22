namespace Lsquad.Setting;

public interface ISettingFacade
{
    public int GetStatusForSetting(string settingName);

    public bool AreAllStatusesReady();

    public void SetStatus(string settingName, int status);
}
