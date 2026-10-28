using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Assets;

public sealed class DbConnectionProvider
{
    private readonly string _connectionString;

    public DbConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}