using System.Threading.Tasks;
using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Microservices.Authorization.Contract
{
    public interface IDeviceAuthService
    {
        Task<AuthResponse> CanReadDevice(DeviceAuthRequest request);
        Task<AuthResponse> CanWriteDevice(DeviceAuthRequest request);
    }
}
