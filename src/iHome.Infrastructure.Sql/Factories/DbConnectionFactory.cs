using Microsoft.Data.SqlClient;
using System.Data;

namespace iHome.Infrastructure.Sql.Factories;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
    IDbConnection GetOpenConnection();
}

internal class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public IDbConnection GetOpenConnection()
    {
        using var conn = GetConnection();
        conn.Open();

        return conn;
    }
}
