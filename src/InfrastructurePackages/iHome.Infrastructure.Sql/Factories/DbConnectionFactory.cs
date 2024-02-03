using iHome.Infrastructure.Sql.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iHome.Infrastructure.Sql.Factories;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}

internal class DbConnectionFactory : IDbConnectionFactory
{
    private readonly DbConnectionFactoryOptions _options;

    public DbConnectionFactory(DbConnectionFactoryOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _options = options;
    }

    public IDbConnection GetConnection()
    {
        if (string.IsNullOrEmpty(_options.ConnectionString))
        {
            throw new ArgumentNullException(nameof(_options.ConnectionString));
        }

        return new SqlConnection(_options.ConnectionString);
    }
}
