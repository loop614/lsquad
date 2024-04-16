using Npgsql;
using Lsquad.Core.Persistence;
using Lsquad.Core.Transfer;
using Lsquad.Core;
using Lsquad.Core.Domain;
using Lsquad.DataImport;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
CoreConfig.AddBuilderServices(builder);

// TODO: consider hangfire
// builder.Services.AddHostedService<LsquadStartupService>();
var app = builder.Build();
app.MapControllers();

var settingsSql = builder.Configuration.GetSection("Sql").Get<LsquadSqlSettings>();
if (settingsSql is null)
{
    Console.WriteLine("Error: Could not load sql settings");
    return;
}

var sqlConnection = new NpgsqlConnection(settingsSql.ConnectionString);
sqlConnection.Open();
if (sqlConnection.State != System.Data.ConnectionState.Open)
{
    Console.WriteLine("Error: Could not connect to sqlpostgres");
    return;
}

// TODO: remove, restart database schema
await LsquadCoreDatabaseClean.DropTables(sqlConnection);
await LsquadCoreDatabaseInit.InitTables(sqlConnection);
sqlConnection.Close();

Task taskImportExampleData = new(() =>
{
    var importer = app.Services.GetService<IDataImportService>();
    if (importer is null) {
        Console.WriteLine("Sadly the data importer service could not construct");
        return;
    }
    importer.ImportExample();
});

taskImportExampleData.Start();

app.Run();
