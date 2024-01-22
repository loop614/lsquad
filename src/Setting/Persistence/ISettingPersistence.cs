namespace Lsquad.Setting.Persistence;

public interface ISettingPersistence
{
    public int GetStatusForSetting(string settingName);

    public bool AreAllStatusesReady();

    public void SetStatus(string settingName, int status);
}
