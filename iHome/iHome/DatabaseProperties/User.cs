namespace iHome.DatabaseProperties
{
    public class User
    {
        public int Id;
        public String Login;
        public String Password;
        public User(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
        public override string ToString()
        {
            return "{\"Id\":" + Id + ",\"Login\":" + Login + "}";
        }
    }
}
