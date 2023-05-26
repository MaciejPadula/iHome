using iHome.Microservices.Widgets.Contract.Models.Request;
using iHome.Microservices.Widgets.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Widgets.Contract
{
    public interface IWidgetDeviceManagementService
    {
        Task InsertDevice(InsertDeviceRequest request);
        Task RemoveDevice(RemoveDeviceRequest request);
        Task<GetWidgetDevicesIdsResponse> GetWidgetDevicesIds(GetWidgetDevicesIdsRequest request);
    }
}
