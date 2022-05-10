namespace iHome.DatabaseProperties
{
    public class DatabaseConfiguration
    {
        private string url;
        private string login;
        private string password;
        private string db;
        public DatabaseConfiguration(string url, string login, string password, string db)
        {
            this.url = url;
            this.login = login;
            this.password = password;
            this.db = db;
        }
        public string GetUrl()
        {
            return url;
        }
        public string GetLogin()
        {
            return login;
        }
        public string GetPassword()
        {
            return password;
        }
        public string GetDatabase()
        {
            return db;
        }
    }
}
