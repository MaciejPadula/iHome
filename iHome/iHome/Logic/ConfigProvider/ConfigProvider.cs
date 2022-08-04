using iHome.Models.Database;
using Newtonsoft.Json;

namespace iHome.Logic.ConfigProvider
{
    public class ConfigProvider: IConfigProvider
    {
        public ApplicationSettings loadDatabaseSettings(string? filename)
        {
            var settings = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(File.ReadAllText(filename));
            return new ApplicationSettings
            {
                AzureConnectionString = settings["AzureConnectionString"],
            };
        }
    }
}
