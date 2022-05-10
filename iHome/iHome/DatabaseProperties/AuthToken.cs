namespace iHome.DatabaseProperties
{
    public class AuthToken
    {
        public int Id;
        public int UserId;
        public DateTime ExpirationDate;
        public Guid AuthKey;

        public AuthToken(int id, int userId, DateTime expirationDate, Guid authKey)
        {
            Id = id;
            UserId = userId;
            ExpirationDate = expirationDate;
            AuthKey = authKey;
         }
    }
}
