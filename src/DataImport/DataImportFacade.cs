namespace Lsquad.DataImport;

public class DataImportFacade : IDataImportFacade
{
    private readonly DataImportFactory _factory = new();

    public void ImportExample()
    {
        _factory.CreateImporter().ImportExample();
    }
}
