using iHome.Core.Logic.Database;
using iHome.Core.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace iHome.AzureFunctions.Logic
{
    public class GetDeviceData
    {
        private readonly MemoryCache _memoryCache;
        private readonly AppDbContext _dbContext;

        public GetDeviceData(AppDbContext dbContext, MemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        [FunctionName("GetDeviceData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var requestData = JsonConvert.DeserializeObject<DeviceDataRequest>(requestBody);
            var deviceId = requestData.DeviceId;

            var cacheObject = _memoryCache.Get(deviceId);
            if (cacheObject != null)
                return new OkObjectResult(cacheObject);

            var deviceToRead = await _dbContext.Devices.Where(device => device.DeviceId == deviceId).FirstOrDefaultAsync();
            _memoryCache.Set<string>(deviceId, deviceToRead.Data, TimeSpan.FromMinutes(60));

            if (deviceToRead == null)
                return new NotFoundResult();

            return new OkObjectResult(deviceToRead.Data);
        }
    }
}
