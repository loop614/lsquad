using Npgsql;

namespace Lsquad.Core.Persistence;

public class LsquadPersistence
{
    private static int connectionIndex = 0;

    private static readonly NpgsqlConnection[] connections = new NpgsqlConnection[3];

    private static bool init = true;

    public LsquadPersistence()
    {
        if (init) InitSqlConnections();
        init = false;
    }

    private static void InitSqlConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = new NpgsqlConnection("Host=localhost;Username=lsquad;Password=lsquad;Database=lsquad");
        }
    }

    protected NpgsqlConnection GetConnection()
    {
        if (connectionIndex + 1 > connections.Length - 2)
        {
            connectionIndex = 0;
        }

        return connections[connectionIndex];
    }

    protected NpgsqlConnection GetReadConnection()
    {
        return connections[^1];
    }
}
