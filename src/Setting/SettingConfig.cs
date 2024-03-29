using Lsquad.Setting.Persistence;

namespace Lsquad.Setting;

public class SettingConfig
{
    // TODO: should be enum but found it to be more flexible using Dictionary instead
    private static Dictionary<int, string> settingStatus = new()
    {
        { 0, STATUS_NEW },
        { 1, STATUS_IN_PROGRESS },
        { 2, STATUS_DONE },
    };

    public const string STATUS_NEW = "new";

    public const string STATUS_IN_PROGRESS = "in progress";

    public const string STATUS_DONE = "done";


    public static string? GetStatusName(int key)
    {
        settingStatus.TryGetValue(key, out string? value);
        return value;
    }

    public static int? GetStatusKey(string key)
    {
        return settingStatus.FirstOrDefault(x => x.Value == key).Key;
    }

    public static int GetStatusDone()
    {
        return settingStatus.FirstOrDefault(x => x.Value == STATUS_DONE).Key;
    }

    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ISettingPersistence, SettingPersistence>();
        builder.Services.AddTransient<ISettingService, SettingService>();
    }
}
