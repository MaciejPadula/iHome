using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Devices.Contract
{
    public interface IDeviceConfigurationService
    {
        Task<GetDevicesToConfigureResponse> GetDevicesToConfigure(GetDevicesToConfigureRequest request);
        Task RemoveDeviceToConfigure(RemoveDeviceToConfigureRequest request);
    }
}
