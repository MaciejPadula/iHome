using iHome.Microservices.Schedules.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace iHome.Microservices.Schedules.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}

public class SqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlDbConnectionFactory(IOptions<ConnectionStrings> options)
    {
        _connectionString = options.Value.SqlConnectionString;
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
