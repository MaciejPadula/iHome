using iHome.Core.Logic.Database;
using iHome.Core.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

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

            _dbContext.DevicesToConfigure.Add(new TDeviceToConfigure(Guid.NewGuid(), deviceId, deviceType, ip));
            _dbContext.SaveChanges();

            return new OkResult();
        }
    }
}