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
    public static class SetDeviceData
    {
        
        [FunctionName("SetDeviceData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            DatabaseModel databaseModel = new DatabaseModel("ihome.database.windows.net", "rootAdmin", "VcuraBEFKR6@3PX", "iHome");

            log.LogInformation("C# HTTP trigger function processed a request.");

            string deviceId = req.Query["deviceId"];
            string deviceData = req.Query["deviceData"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            deviceId = deviceId ?? data?.deviceId;
            deviceData = deviceData ?? data?.deviceData;

            string responseMessage = string.IsNullOrEmpty(deviceId)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {deviceId}. This HTTP triggered function executed successfully.";
            if(databaseModel.SetDeviceData(deviceId, deviceData))
                return new OkObjectResult(responseMessage);
            return new NotFoundResult();
        }
    }
}
