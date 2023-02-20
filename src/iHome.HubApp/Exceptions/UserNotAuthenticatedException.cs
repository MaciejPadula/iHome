namespace iHome.HubApp.Exceptions;

public class UserNotAuthenticatedException : UIException
{
    public UserNotAuthenticatedException() { }

    public UserNotAuthenticatedException(string info): base(info) { }
}
