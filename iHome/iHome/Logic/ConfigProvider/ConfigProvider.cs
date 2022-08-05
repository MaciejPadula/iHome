using iHome.Models.Application;
using Newtonsoft.Json;

namespace iHome.Logic.ConfigProvider
{
    public class ConfigProvider: IConfigProvider
    {
        private ApplicationSettings _config = new ApplicationSettings();
        public ApplicationSettings Configuration 
        {
            get
            {
                return _config;
            } 
            private set 
            {
                _config = value; 
            } 
        }

        public ConfigProvider()
        {
            LoadDatabaseSettings("appsettings.json");
        }

        public void LoadDatabaseSettings(string filename)
        {
            var config = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(filename));
            if (config != null)
            {
                Configuration.AzureConnectionString = config.AzureConnectionString;
            }
        }
    }
}
