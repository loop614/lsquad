namespace Lsquad.Language;

public class LanguageFacade : ILanguageFacade
{
    private readonly LanguageFactory _factory;

    public LanguageFacade()
    {
        _factory = new LanguageFactory();
    }

    public Dictionary<string, int> GetOrCreate(List<string> langs)
    {
        return _factory.CreateLanguagePersistence().GetOrCreate(langs);
    }

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs)
    {
        return _factory.CreateLanguagePersistence().GetLanguagesNameToId(langs);
    }
}
