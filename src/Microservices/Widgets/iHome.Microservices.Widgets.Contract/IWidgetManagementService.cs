using iHome.Microservices.Widgets.Contract.Models.Request;
using iHome.Microservices.Widgets.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Widgets.Contract
{
    public interface IWidgetManagementService
    {
        Task<AddWidgetResponse> AddWidget(AddWidgetRequest request);
        Task<GetWidgetsResponse> GetWidgets(GetWidgetsRequest request);
        Task RemoveWidget(RemoveWidgetRequest request);
    }
}
