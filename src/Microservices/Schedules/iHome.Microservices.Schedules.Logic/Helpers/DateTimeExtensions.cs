namespace iHome.Microservices.Schedules.Logic.Helpers;

public static class DateTimeExtensions
{
    public static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
    }
}
