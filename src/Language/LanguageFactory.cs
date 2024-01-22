using Lsquad.Language.Persistence;

namespace Lsquad.Language;

public class LanguageFactory
{
    public LanguagePersistence CreateLanguagePersistence()
    {
        return new LanguagePersistence();
    }
}
