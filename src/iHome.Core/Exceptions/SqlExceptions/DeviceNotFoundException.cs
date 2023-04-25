namespace iHome.Core.Exceptions.SqlExceptions;

public class DeviceNotFoundException : SqlException
{
    public override string SpecialMessage => $"Can't find your device";
}
