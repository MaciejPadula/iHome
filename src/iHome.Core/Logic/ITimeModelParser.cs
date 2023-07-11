using iHome.Core.Models;

namespace iHome.Core.Logic;

public interface ITimeModelParser
{
    TimeModel Parse(string input);
}
