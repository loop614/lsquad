namespace Lsquad.Language.Persistence;

public interface ILanguagePersistence
{
    public Dictionary<string, int> GetOrCreate(List<string> langs);

    public Dictionary<string, int> GetLanguagesNameToId(List<string> langs);
}
