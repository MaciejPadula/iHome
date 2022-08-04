using iHome.Models.Database;

namespace iHome.Logic.ConfigProvider
{
    public interface IConfigProvider
    {
        ApplicationSettings loadDatabaseSettings(string? filename);
    }
}
