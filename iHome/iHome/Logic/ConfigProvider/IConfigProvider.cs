using iHome.Models.Application;

namespace iHome.Logic.ConfigProvider
{
    public interface IConfigProvider
    {
        ApplicationSettings Configuration { get; }
        void LoadDatabaseSettings(string filename);
    }
}
