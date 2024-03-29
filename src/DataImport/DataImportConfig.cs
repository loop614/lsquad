using Lsquad.DataImport.Domain;

namespace Lsquad.DataImport;

public class DataImportConfig
{
    public static void AddBuilderServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IImporter, Importer>();
        builder.Services.AddTransient<IDispatcher, Dispatcher>();
        builder.Services.AddTransient<IDataImportService, DataImportService>();
    }
}
