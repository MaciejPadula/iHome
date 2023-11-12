using iHome.Model;

namespace iHome.Core.Logic;

public class TimeModelParser : ITimeModelParser
{
    public TimeModel Parse(string input)
    {
        var segments = input?.Split(":") ?? Array.Empty<string>();

        if (segments.Length != 2 
            || !int.TryParse(segments[0], out var hour)
            || !int.TryParse(segments[1], out var minute))
        {
            return default;
        }

        return new TimeModel(hour, minute, true);
    }
}
