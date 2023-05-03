namespace iHome.Logic.Notificator
{
    public interface INotificator
    {
        void NotifyUser(string uuid);
        void NotifyUsers(List<string> uuids);
        void NotifyUsers(List<string> uuids, List<string> except);
    }
}
