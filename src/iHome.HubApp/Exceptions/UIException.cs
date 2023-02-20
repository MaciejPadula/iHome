using System;

namespace iHome.HubApp.Exceptions;

public class UIException : Exception
{
    public UIException() { }
    public UIException(string info) : base(info) { }
}
