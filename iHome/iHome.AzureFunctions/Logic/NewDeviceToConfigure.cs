using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using iHome.Core.Logic.Database;

namespace iHome.AzureFunctions.Logic
{
    public class NewDeviceToConfigure
    {
        private readonly AppDbContext _dbContext;

        public NewDeviceToConfigure(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("NewDeviceToConfigure")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var deviceId = (string)data?.deviceId;
            var deviceType = (int)data?.deviceType;
            var ip = req.HttpContext.Connection.RemoteIpAddress.ToString();

            _dbContext.DevicesToConfigure.Add(new() { DeviceId = deviceId, DeviceType = deviceType, IpAddress = ip});
            _dbContext.SaveChanges();

            return new OkObjectResult(deviceId);
        }
    }
}
