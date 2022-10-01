using iHome.Core.Logic.Database;
using iHome.Core.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace iHome.AzureFunctions.Logic
{
    public class SetDeviceData
    {
        private readonly MemoryCache _memoryCache;
        private readonly AppDbContext _dbContext;

        public SetDeviceData(AppDbContext dbContext, MemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        [FunctionName("SetDeviceData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<DeviceDataRequest>(requestBody);
            var deviceId = data.DeviceId;
            var deviceData = data.DeviceData;
            _memoryCache.Set<string>(deviceId, null);
            var deviceToUpdate = _dbContext.Devices.Where(device => device.DeviceId == deviceId).FirstOrDefault();

            if (deviceToUpdate == null)
                return new NotFoundResult();

            deviceToUpdate.Data = deviceData;
            _dbContext.SaveChanges();

            return new OkResult();
        }
    }
}
