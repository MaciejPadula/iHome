using iHome.Model;

namespace iHome.Shared.Logic;

public interface ITimeModelParser
{
    TimeModel Parse(string input);
}
