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
using System.Net.Http;

namespace iHomeAzureFunctions
{
    public static class NewDeviceToConfigure
    {
        [FunctionName("NewDeviceToConfigure")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin,  "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            
            DatabaseModel databaseModel = new DatabaseModel("ihomedevice.database.windows.net", "tritIouR", "88swNNgWXt2jr5F", "ihomedevice");

            log.LogInformation("C# HTTP trigger function processed a request.");

            string deviceId = req.Query["deviceId"];
            string deviceType = req.Query["deviceType"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            deviceId = deviceId ?? data?.deviceId;
            deviceType = deviceType ?? data?.deviceType;

            var httpClient = new HttpClient();
            var ip = req.HttpContext.Connection.RemoteIpAddress.ToString();
            //await httpClient.GetStringAsync("https://api.ipify.org");



            bool deviceData = await databaseModel.AddNewDeviceToConfigureAsync(deviceId, int.Parse(deviceType), ip);

            return new OkObjectResult(deviceData);
        }
    }
}
