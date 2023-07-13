using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Authorization.Contract
{
    public interface IWidgetAuthService
    {
        Task<AuthResponse> CanReadWidget(WidgetAuthRequest request);
        Task<AuthResponse> CanWriteWidget(WidgetAuthRequest request);
    }
}
