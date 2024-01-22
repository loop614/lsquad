using Dapper;
using Lsquad.Core.Persistence;
using Lsquad.Language.Transfer;

namespace Lsquad.Language.Persistence;

public class LanguagePersistence : LsquadPersistence, ILanguagePersistence
{
    private static Dictionary<string, int> LanguagesCache = [];

    public Dictionary<string, int> GetOrCreate(List<string> langs)
    {
        Dictionary<string, int> languageNameToId = [];
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

        List<LanguageEntity> languageEntities = CreateOrUpdateLanguagesByName(langsToAdd);
        foreach(LanguageEntity languageEntity in languageEntities) {
            languageNameToId[languageEntity.name] = languageEntity.id_language;
            LanguagesCache[languageEntity.name] = languageEntity.id_language;
        }

        return languageNameToId;
    }

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs)
    {
        Dictionary<string, int> languageNameToId = [];
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

        List<LanguageEntity> languageEntities = GetLanguages(notInCacheLangs);
        foreach(LanguageEntity languageEntity in languageEntities) {
            languageNameToId[languageEntity.name] = languageEntity.id_language;
            LanguagesCache[languageEntity.name] = languageEntity.id_language;
        }

        return languageNameToId;
    }

    private List<LanguageEntity> CreateOrUpdateLanguagesByName(List<string> languages)
    {
        string sql =
            $@"INSERT INTO lsquad_language (name) VALUES {ListToManyValues(languages)} " +
            "  ON CONFLICT (name) DO UPDATE SET name = excluded.name" +
            "  RETURNING *";
        List<LanguageEntity> languageEntities = [];
        foreach(string language in languages) {
            LanguageEntity le = new() { name = language };
            languageEntities.Add(le);
        }
        Console.WriteLine($"running {sql} with {languageEntities.Count} languageEntities");

        return GetConnection().Query<LanguageEntity>(sql).ToList();
    }

    private List<LanguageEntity> GetLanguages(List<string> langs)
    {
        const string sql = @"SELECT * FROM lsquad_language WHERE name = ANY(@langs)";
        var parameters = new { langs };
        Console.WriteLine($"running {sql} with {parameters}");

        return GetConnection().Query<LanguageEntity>(sql, parameters).ToList();
    }
}
