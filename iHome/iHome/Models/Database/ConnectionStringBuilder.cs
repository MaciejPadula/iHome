using System.Data.SqlClient;

namespace iHome.Models.Database
{
    public class ConnectionStringBuilder
    {
        SqlConnectionStringBuilder builder;

        public ConnectionStringBuilder(string server)
        {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = server;
        }
        public ConnectionStringBuilder withLogin(string login)
        {
            builder.UserID = login;
            return this;
        }
        public ConnectionStringBuilder withPassword(string password)
        {
            builder.Password = password;
            return this;
        }
        public ConnectionStringBuilder withInitialCatalog(string db)
        {
            builder.InitialCatalog = db;
            return this;
        }
        public string build()
        {
            return builder.ConnectionString;
        }
    }
}
