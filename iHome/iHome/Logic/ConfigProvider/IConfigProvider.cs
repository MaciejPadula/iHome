using iHome.Models.Database;

namespace iHome.Logic.ConfigProvider
{
    public interface IConfigProvider
    {
        DatabaseSettings loadDatabaseSettings(string? filename);
    }
}
