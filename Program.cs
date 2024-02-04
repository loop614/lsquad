using Npgsql;
using Lsquad.Core.Persistence;
using Lsquad.Core.Transfer;
using Lsquad.DataImport;
using Lsquad.Squad;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
var app = builder.Build();
app.MapControllers();

var settingsSql = builder.Configuration.GetSection("Sql").Get<LsquadSqlSettings>();
if (settingsSql is null) {
    Console.WriteLine("Error: Could not load sql settings");
    return;
}

var sqlConnection = new NpgsqlConnection(settingsSql.ConnectionString);
sqlConnection.Open();
if (sqlConnection.State != System.Data.ConnectionState.Open) {
    Console.WriteLine("Error: Could not connect to sqlpostgres");
    return;
}

// Restart database schema
await LsquadCoreDatabaseClean.DropTables(sqlConnection);
await LsquadCoreDatabaseInit.InitTables(sqlConnection);

// Run local example from dataimport:
new DataImportFacade().ImportExample();
new SquadFacade().GetSquad(23400, "el");

app.Run();
