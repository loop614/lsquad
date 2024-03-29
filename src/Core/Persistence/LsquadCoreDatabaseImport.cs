using Lsquad.DataImport;

namespace Lsquad.Core.Persistence;

class LsquadCoreDatabaseImport(IServiceScopeFactory scopeFactory)
{
    public void ImportExampleData()
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var dataImportService = scope.ServiceProvider.GetRequiredService<IDataImportService>();
            dataImportService.ImportExample();
        }
    }
}
