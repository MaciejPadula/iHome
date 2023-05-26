namespace iHome.Jobs.Events.Infrastructure.Helpers;

public static class DateTimeExtensions
{
    public static DateTime StartOfDay(this DateTime theDate)
    {
        return theDate.Date;
    }

    public static DateTime EndOfDay(this DateTime theDate)
    {
        return theDate.Date.AddDays(1).AddTicks(-1);
    }

    public static bool EarlierThan(this DateTime earlierDate, DateTime LaterDate)
    {
        return DateTime.Compare(earlierDate, LaterDate) < 0;
    }
}
