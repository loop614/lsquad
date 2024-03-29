using Lsquad.Language.Persistence;

namespace Lsquad.Language;

public class LanguageConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ILanguagePersistence, LanguagePersistence>();
        builder.Services.AddTransient<ILanguageService, LanguageService>();
    }
}
