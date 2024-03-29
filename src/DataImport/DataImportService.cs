using Lsquad.DataImport.Domain;

namespace Lsquad.DataImport;

public class DataImportService(IImporter importer) : IDataImportService
{
    public void ImportExample()
    {
        importer.ImportExample();
    }
}
