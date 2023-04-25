using Cronos;
using System.Text;

namespace iHome.Shared.Logic;

public class CronHelper
{
    public static string CreateCronExpressions(int day, int hour, int minutes)
    {
        var builder = new StringBuilder("");

        builder.Append(minutes);
        builder.Append(" ");

        builder.Append(hour);
        builder.Append(" * * *");

        return builder.ToString();
    }

    public static DateTime? GetNextOccurence(string cronExpression, DateTime now)
    {
        var cron = CronExpression.Parse(cronExpression);
        var occurence = cron.GetNextOccurrence(now);

        return occurence ?? null;
    }
}
