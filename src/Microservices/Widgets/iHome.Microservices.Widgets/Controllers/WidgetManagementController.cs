using iHome.Core.Repositories.Widgets;
using iHome.Microservices.Widgets.Contract;
using iHome.Microservices.Widgets.Contract.Models.Request;
using iHome.Microservices.Widgets.Contract.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Widgets.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WidgetManagementController : ControllerBase, IWidgetManagementService
    {
        private readonly IWidgetRepository _widgetRepository;

        public WidgetManagementController([FromBody] IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        [HttpPost]
        public Task AddWidget([FromBody] AddWidgetRequest request)
        {
            //validation

            return _widgetRepository.Add(request.Type, request.RoomId, request.ShowBorder);
        }

        [HttpPost]
        public async Task<GetWidgetsResponse> GetWidgets([FromBody] GetWidgetsRequest request)
        {
            //validation

            return new()
            {
                Widgets = await _widgetRepository.GetByRoomId(request.RoomId)
            };
        }

        [HttpPost]
        public Task RemoveWidget([FromBody] RemoveWidgetRequest request)
        {
            // validation

            return _widgetRepository.Remove(request.WidgetId);
        }
    }
}
