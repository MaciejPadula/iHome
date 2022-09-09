using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using iHome.Core.Logic.Database;
using System.Linq;

namespace iHome.AzureFunctions.Logic
{
    public class GetDeviceData
    {
        private readonly AppDbContext _dbContext;

        public GetDeviceData(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("GetDeviceData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var deviceId = (string)data?.deviceId;

            var deviceToRead = _dbContext.Devices.Where(device => device.DeviceId == deviceId).FirstOrDefault();

            return new OkObjectResult(deviceToRead.Data);
        }
    }
}
