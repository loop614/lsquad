using Npgsql;

namespace Lsquad.Core.Persistence;

public class LsquadPersistence
{
    protected static int _connectionIndex;

    protected static readonly int _connectionNumber = 10;

    static protected NpgsqlConnection[] _connections = [];

    static protected bool _init = true;

    public LsquadPersistence()
    {
        if (_init) {
            InitSqlConnections();
        }
        _init = false;
    }

    private void InitSqlConnections()
    {
        _connections = new NpgsqlConnection[_connectionNumber];
        for(int i = 0; i < _connectionNumber; i++) {
            _connections[i] = new NpgsqlConnection("Host=localhost;Username=lsquad;Password=lsquad;Database=lsquad");
        }
    }

    protected NpgsqlConnection GetConnection()
    {
        if (_connectionIndex + 1 > _connections.Count() - 1) {
            _connectionIndex = 0;
        }

        return _connections[_connectionIndex++];
    }

    protected string ListToManyValues(List<string> values)
    {
        List<string> valuesWithBrackets = [];
        foreach(string value in values) {
            valuesWithBrackets.Add("('" + value + "')");
        }

        return String.Join(", ", valuesWithBrackets);
    }

    protected string ListToManyValues(List<int> values)
    {
        List<string> valuesWithBrackets = [];
        foreach(int value in values) {
            valuesWithBrackets.Add("(" + value.ToString() + ")");
        }

        return String.Join(", ", valuesWithBrackets);
    }
}
