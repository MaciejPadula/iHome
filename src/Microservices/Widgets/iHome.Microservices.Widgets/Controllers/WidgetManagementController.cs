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

        public WidgetManagementController(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        [HttpPost]
        public Task AddWidget(AddWidgetRequest request)
        {
            //validation

            return _widgetRepository.Add(request.Type, request.RoomId, request.ShowBorder);
        }

        [HttpPost]
        public async Task<GetWidgetsResponse> GetWidgets(GetWidgetsRequest request)
        {
            //validation

            return new()
            {
                Widgets = await _widgetRepository.GetByRoomId(request.RoomId)
            };
        }

        [HttpPost]
        public Task RemoveWidget(RemoveWidgetRequest request)
        {
            // validation

            return _widgetRepository.Remove(request.WidgetId);
        }
    }
}
