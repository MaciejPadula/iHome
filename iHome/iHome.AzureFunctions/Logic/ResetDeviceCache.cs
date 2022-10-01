using iHome.Core.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace iHome.AzureFunctions.Logic
{
    public class ResetDeviceCache
    {
        private readonly MemoryCache _memoryCache;

        public ResetDeviceCache(MemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [FunctionName("ResetDeviceCache")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<DeviceDataRequest>(requestBody);
            var deviceId = data.DeviceId;

            _memoryCache.Set<string>(deviceId, null);

            return new OkResult();
        }
    }
}
