using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using iHomeAzureFunctions.Models;

namespace iHomeAzureFunctions
{
    public static class GetDeviceData
    {
        [FunctionName("GetDeviceData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            DatabaseModel databaseModel = new DatabaseModel("ihome.database.windows.net", "rootAdmin", "VcuraBEFKR6@3PX", "iHome");
            log.LogInformation("C# HTTP trigger function processed a request.");

            string deviceId = req.Query["DeviceId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            deviceId = deviceId ?? data?.DeviceId;

            dynamic deviceData = databaseModel.GetDeviceData(deviceId);

            return new OkObjectResult(deviceData);
        }
    }
}
