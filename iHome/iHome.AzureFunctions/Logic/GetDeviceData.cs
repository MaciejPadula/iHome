using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using iHome.Core.Logic.Database;
using System.Linq;
using iHome.Core.Models.Requests;

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
            var requestData = JsonConvert.DeserializeObject<DeviceDataRequest>(requestBody);
            var deviceId = requestData.DeviceId;
            var deviceToRead = _dbContext.Devices.Where(device => device.DeviceId == deviceId).FirstOrDefault();

            if (deviceToRead == null)
                return new NotFoundResult();

            return new OkObjectResult(deviceToRead.Data);
        }
    }
}
