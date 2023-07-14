using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Authorization.Contract
{
    public interface IScheduleAuthService
    {
        Task<AuthResponse> CanAccessSchedule(ScheduleAuthRequest request);
    }
}
