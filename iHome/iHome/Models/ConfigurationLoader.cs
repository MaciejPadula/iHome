using iHome.Models.Database;
using Newtonsoft.Json;

namespace iHome.Models
{
    public class ConfigurationLoader
    {
        public DatabaseSettings loadDatabaseSettings(string? filename)
        {
            var settings = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(File.ReadAllText(filename));
            return new DatabaseSettings
            {
                DatabaseServer = settings["Azure"].Server,
                DatabaseLogin = settings["Azure"].Login,
                DatabasePassword = settings["Azure"].Password,
                DatabaseName = settings["Azure"].Database
            };
        }
    }
}
