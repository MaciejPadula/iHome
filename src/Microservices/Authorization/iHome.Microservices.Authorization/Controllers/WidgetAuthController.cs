using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using iHome.Microservices.Authorization.Managers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Authorization.Controllers
{
    public class WidgetAuthController : ControllerBase, IWidgetAuthService
    {
        private readonly IWidgetManager _widgetManager;

        public WidgetAuthController(IWidgetManager widgetManager)
        {
            _widgetManager = widgetManager;
        }

        [HttpPost]
        public async Task<AuthResponse> CanReadWidget([FromBody] WidgetAuthRequest request)
        {
            return new()
            {
                HasAccess = await _widgetManager.CanRead(request)
            };
        }

        [HttpPost]
        public async Task<AuthResponse> CanWriteWidget([FromBody] WidgetAuthRequest request)
        {
            return new()
            {
                HasAccess = await _widgetManager.CanWrite(request)
            };
        }
    }
}
