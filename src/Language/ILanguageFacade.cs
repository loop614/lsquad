namespace Lsquad.Language;

public interface ILanguageFacade
{
    public Dictionary<string, int> GetOrCreate(List<string> langs);

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs);
}
