using Web.Infrastructure.Microservices.Client.Interfaces;

namespace iHome
{
    public class ConfigurationServiceLookup : IServiceLookup
    {
        private readonly IConfiguration _configuration;

        public ConfigurationServiceLookup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Uri Lookup(string serviceName)
        {
            return new Uri(_configuration[$"Microservices:{serviceName}"] ?? string.Empty);
        }
    }
}
