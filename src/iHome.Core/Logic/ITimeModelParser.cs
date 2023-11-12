using iHome.Model;

namespace iHome.Core.Logic;

public interface ITimeModelParser
{
    TimeModel Parse(string input);
}
