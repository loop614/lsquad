using Lsquad.DataImport;

namespace Lsquad.Core.Domain;

public class LsquadStartupService(IServiceScopeFactory scopeFactory) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var dataImportService = scope.ServiceProvider.GetRequiredService<IDataImportService>();
            dataImportService.ImportExample();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
