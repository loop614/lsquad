using Lsquad.Language.Persistence;

namespace Lsquad.Language;

public class LanguageService(ILanguagePersistence languagePersistence) : ILanguageService
{
    public Dictionary<string, int> GetOrCreate(List<string> langs)
    {
        return languagePersistence.GetOrCreate(langs);
    }

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs)
    {
        return languagePersistence.GetLanguagesNameToId(langs);
    }
}
