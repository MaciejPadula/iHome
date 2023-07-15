using Firebase.Database;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Devices.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace iHome.Microservices.Devices.Infrastructure.Repositories
{
    public class FirebaseDeviceConfigurationRepository : IDeviceConfigurationRepository
    {
        private readonly FirebaseClient _client;

        private const string IndexName = "devicesToConfigure";

        public FirebaseDeviceConfigurationRepository(IOptions<FirebaseSettings> options)
        {
            _client = new FirebaseClient(options.Value.Url);
        }

        public async Task<IEnumerable<DeviceToConfigure>> GetDeviceToConfigure(string ipAddress)
        {
            var retrivedData = await _client
                .Child($"/{IndexName}/{ipAddress}")
                .OnceAsListAsync<DeviceToConfigure>();

            return retrivedData
                .Select(o => o.Object);
        }

        public async Task RemoveDeviceToConfigure(string ipAddress, Guid id)
        {
            await _client
                .Child($"/{IndexName}/{ipAddress}")
                .DeleteAsync();
        }
    }
}
