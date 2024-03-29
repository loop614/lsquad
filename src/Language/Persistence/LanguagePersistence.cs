using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Language.Transfer;

namespace Lsquad.Language.Persistence;

public class LanguagePersistence : LsquadPersistence, ILanguagePersistence
{
    private static Dictionary<string, int> LanguagesCache = new();

    public Dictionary<string, int> GetOrCreate(List<string> langs)
    {
        Dictionary<string, int> languageNameToId = new();
        List<string> langsToAdd = [];
        foreach(string lang in langs) {
            if (LanguagesCache.Keys.Contains(lang)) {
                languageNameToId[lang] = LanguagesCache[lang];
            } else {
                langsToAdd.Add(lang);
            }
        }
        if (langsToAdd.Count == 0) {
            return languageNameToId;
        }

        List<LanguageTransfer> languageTransfers = CreateOrUpdateLanguagesByName(langsToAdd);
        foreach(LanguageTransfer languageTransfer in languageTransfers) {
            languageNameToId[languageTransfer.name] = languageTransfer.id_language;
            LanguagesCache[languageTransfer.name] = languageTransfer.id_language;
        }

        return languageNameToId;
    }

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs)
    {
        Dictionary<string, int> languageNameToId = new();
        List<string> notInCacheLangs = [];
        foreach(string lang in langs) {
            if (LanguagesCache.ContainsKey(lang)) {
                languageNameToId[lang] = LanguagesCache[lang];
            } else {
                notInCacheLangs.Add(lang);
            }
        }
        if (notInCacheLangs.Count == 0) {
            return languageNameToId;
        }

        List<LanguageTransfer> languageTransfers = GetLanguages(notInCacheLangs);
        foreach(LanguageTransfer languageTransfer in languageTransfers) {
            languageNameToId[languageTransfer.name] = languageTransfer.id_language;
            LanguagesCache[languageTransfer.name] = languageTransfer.id_language;
        }

        return languageNameToId;
    }

    private List<LanguageTransfer> CreateOrUpdateLanguagesByName(List<string> languages)
    {
        string sql =
            $@"INSERT INTO lsquad_language (name) VALUES {ListToManyValues(languages)} " +
            "  ON CONFLICT (name) DO UPDATE SET name = excluded.name" +
            "  RETURNING *";
        List<LanguageTransfer> languageTransfers = [];
        foreach(string language in languages) {
            LanguageTransfer le = new() { name = language };
            languageTransfers.Add(le);
        }
        Console.WriteLine($"running {sql} with {languageTransfers.Count} languageTransfers");

        return GetConnection().Query<LanguageTransfer>(sql).ToList();
    }

    private List<LanguageTransfer> GetLanguages(List<string> langs)
    {
        const string sql = @"SELECT * FROM lsquad_language WHERE name = ANY(@langs)";
        var parameters = new { langs };
        Console.WriteLine($"running {sql} with {parameters}");

        return GetConnection().Query<LanguageTransfer>(sql, parameters).ToList();
    }
}
