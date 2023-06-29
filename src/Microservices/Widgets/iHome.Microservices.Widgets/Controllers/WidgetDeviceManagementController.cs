using iHome.Microservices.Widgets.Contract;
using iHome.Microservices.Widgets.Contract.Models.Request;
using iHome.Microservices.Widgets.Contract.Models.Response;
using iHome.Microservices.Widgets.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Widgets.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WidgetDeviceManagementController : ControllerBase, IWidgetDeviceManagementService
    {
        private readonly IWidgetDeviceRepository _widgetDeviceRepository;

        public WidgetDeviceManagementController(IWidgetDeviceRepository widgetDeviceRepository)
        {
            _widgetDeviceRepository = widgetDeviceRepository;
        }

        [HttpPost]
        public async Task<GetWidgetDevicesIdsResponse> GetWidgetDevicesIds([FromBody] GetWidgetDevicesIdsRequest request)
        {
            //validation

            return new()
            {
                DevicesIds = await _widgetDeviceRepository.GetDeviceIdsByWidgetId(request.WidgetId)
            };
        }

        [HttpPost]
        public Task InsertDevice([FromBody] InsertDeviceRequest request)
        {
            //validation

            return _widgetDeviceRepository.Add(request.WidgetId, request.DeviceId);
        }

        [HttpPost]
        public Task RemoveDevice([FromBody] RemoveDeviceRequest request)
        {
            //validation

            return _widgetDeviceRepository.Remove(request.WidgetId, request.DeviceId);
        }
    }
}
