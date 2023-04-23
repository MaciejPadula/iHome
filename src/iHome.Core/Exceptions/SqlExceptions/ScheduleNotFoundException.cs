namespace iHome.Core.Exceptions.SqlExceptions;

public class ScheduleNotFoundException : SqlException
{
    public override string SpecialMessage => $"Can't find your schedule";
}
