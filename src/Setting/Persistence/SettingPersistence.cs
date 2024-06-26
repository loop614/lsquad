using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Setting.Transfer;

namespace Lsquad.Setting.Persistence;

public class SettingPersistence : LsquadPersistence, ISettingPersistence
{
    public int? GetStatusForSetting(string settingName)
    {
        const string sql = @"SELECT * FROM lsquad_setting WHERE name = @settingName";
        var parameters = new { settingName };
        Console.WriteLine($"running {sql} with {parameters}");
        var response = GetReadConnection().QueryFirst<SettingTransfer>(sql, parameters);

        return response?.status;
    }

    public bool AreAllStatusesReady()
    {
        const string sql = @"SELECT * FROM lsquad_setting WHERE name = ANY (@names)";
        List<string> names = ["br_domain_player", "br_domain_team", "br_domain_squad"];
        var parameters = new { names };
        List<SettingTransfer> settingList = GetReadConnection().Query<SettingTransfer>(sql, parameters).ToList();

        foreach(SettingTransfer settingTransfer in settingList) {
            if (settingTransfer.status != SettingConfig.GetStatusDone()) {
                return false;
            }
        }

        return true;
    }

    public void SetStatus(string settingName, int status)
    {
        const string sql = @"UPDATE lsquad_setting SET status = @status WHERE name = @settingName";
        var parameters = new { settingName, status };
        Console.WriteLine($"running {sql} with {parameters}");
        GetConnection().Execute(sql, parameters);
    }
}
