using iHome.DatabaseProperties;

namespace iHome.Controllers
{
    public class PageController
    {
        DatabaseConfiguration databaseConfiguration;
        UserController userController;
        public PageController(string url, string login, string password, string db)
        {
            this.databaseConfiguration = new DatabaseConfiguration(url, login, password, db);
            userController = new UserController(this.databaseConfiguration);
        }
        public UserController GetUserController()
        {
            return userController;
        }
    }
}
