using Microsoft.Data.SqlClient;
using System.Data;

namespace iHome.Infrastructure.Sql.Factories;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}

internal class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
