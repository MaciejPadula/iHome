using iHome.Infrastructure.Sql.Factories;
using System.Data;

namespace iHome.Infrastructure.Sql.Repositories;

public class RepositoryBase
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RepositoryBase(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    protected IDbConnection GetDbConnection() => _connectionFactory.GetConnection();
}
