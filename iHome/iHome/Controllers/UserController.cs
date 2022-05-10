using iHome.DatabaseProperties;
using System.Data.SqlClient;

namespace iHome.Controllers
{
    public class UserController
    {
        private SqlConnection sqlConnection;
        public UserController(DatabaseConfiguration databaseConfiguration)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = databaseConfiguration.GetUrl();
            builder.UserID = databaseConfiguration.GetLogin();
            builder.Password = databaseConfiguration.GetPassword();
            builder.InitialCatalog = databaseConfiguration.GetDatabase();

            sqlConnection = new SqlConnection(builder.ConnectionString);
            sqlConnection.Open();
        }
        public int RegisterUser(string login, string password)
        {
            if (password.Length<8)
                return 2;

            if (login.Length < 3)
                return 2;


            bool good = true;
            GetAllUsers().ForEach(user => 
            {
                if (user.Login.Equals(login))
                    good = false;
            });

            if (!good)
                return 3;

            String query = "INSERT INTO [dbo].[user] " +
                "([Login], [Password])" +
                "VALUES" +
                "('"+login+"', '"+ SecurePasswordHasher.Hash(password) + "');";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            if (command.ExecuteNonQuery()!=1)
            {
                return 1;
            }
            return 0;
        }
        public Guid LoginUser(string login, string password)
        {
            
            User userToLogin = null;
            GetAllUsers().ForEach(user => {
                if (user.Login.Equals(login) && SecurePasswordHasher.Verify(password, user.Password))
                {
                    userToLogin = user;
                }
            });
            if(userToLogin == null)
            {
                return Guid.Empty;
            }
            return CreateSession(userToLogin);
        }
        public int CheckSession(Guid authToken)
        {
            int id = -1;
            GetAllValidTokens().ForEach(token =>
            {
                if(token.AuthKey.Equals(authToken))
                {
                    id = token.UserId;
                }
            });
            return id;
        }
        public Guid CreateSession(User user)
        {
            Guid guid = Guid.NewGuid();
            String query = "INSERT INTO [dbo].[Auth] " +
                "([UserId], [AuthKey], [ExpirationDate])" +
                "VALUES" +
                "('" + user.Id + "', '"+guid+"', '" + DateTime.Now.ToString() + "');";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            if (command.ExecuteNonQuery() != 1)
            {
                return Guid.Empty;
            }
            return guid;
        }
        public SqlConnection GetSqlConnection()
        {
            return sqlConnection;
        }
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            String query = "SELECT Id, Login, Password FROM [dbo].[user];";

            SqlCommand command = new SqlCommand(query, sqlConnection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
                reader.Close();
            }

            return users;
        }
        public List<AuthToken> GetAllValidTokens()
        {
            List<AuthToken> tokens = new List<AuthToken>();

            String query = "SELECT Id, UserId, ExpirationDate, AuthKey FROM [dbo].[Auth];";

            SqlCommand command = new SqlCommand(query, sqlConnection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tokens.Add(new AuthToken(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        DateTime.Now,
                        Guid.Parse(reader.GetString(3))
                    ));
                }
            }

            return tokens;
        }
    }
}
