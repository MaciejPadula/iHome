using iHome.Microservices.OpenAI.Contract.Models.Request;
using iHome.Microservices.OpenAI.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.OpenAI.Contract
{
    public interface ISuggestionsService
    {
        Task<GetDevicesThatCouldMatchScheduleResponse> GetDevicesThatCouldMatchSchedule(GetDevicesThatCouldMatchScheduleRequest request);
        Task<GetSuggestedTimeByScheduleNameResponse> GetSuggestedTimeByScheduleName(GetSuggestedTimeByScheduleNameRequest request);
    }
}