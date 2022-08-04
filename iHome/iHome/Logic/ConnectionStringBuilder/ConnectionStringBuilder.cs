using System.Data.SqlClient;

namespace iHome.Logic.ConnectionStringBuilder
{
    public class ConnectionStringBuilder
    {
        SqlConnectionStringBuilder _builder;

        public ConnectionStringBuilder(string server)
        {
            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = server;
        }
        public ConnectionStringBuilder withLogin(string login)
        {
            _builder.UserID = login;
            return this;
        }
        public ConnectionStringBuilder withPassword(string password)
        {
            _builder.Password = password;
            return this;
        }
        public ConnectionStringBuilder withInitialCatalog(string db)
        {
            _builder.InitialCatalog = db;
            return this;
        }
        public string build()
        {
            return _builder.ConnectionString;
        }
    }
}
