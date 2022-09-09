using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using iHome.Core.Logic.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using iHome.Core.Models.Requests;

namespace iHome.AzureFunctions.Logic
{
    public class SetDeviceData
    {
        private readonly AppDbContext _dbContext;

        public SetDeviceData(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("SetDeviceData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<DeviceDataRequest>(requestBody);
            var deviceId = data.DeviceId;
            var deviceData = data.DeviceData;

            var deviceToUpdate = _dbContext.Devices.Where(device => device.DeviceId == deviceId).FirstOrDefault();
            deviceToUpdate.Data = deviceData;
            _dbContext.Entry(deviceToUpdate).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new OkObjectResult(new
            {
                deviceId
            });
        }
    }
}
