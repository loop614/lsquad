namespace Lsquad.Language;

public interface ILanguageService
{
    public Dictionary<string, int> GetOrCreate(List<string> langs);

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs);
}
