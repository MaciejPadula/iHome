using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.Shared.Logic;

namespace iHome.Devices.ApiClient;

public class HttpRoomProvider : IRoomProvider
{
    private readonly JsonHttpClient _httpClient;

    private readonly string BaseApiUrl = "/api/Room";

    public HttpRoomProvider(JsonHttpClient httpClient, ApiClientSettings settings)
    {
        _httpClient = httpClient;
        BaseApiUrl = settings.BaseApiUrl;
    }

    public IEnumerable<GetRoomRequestRoom> GetRoomsForHub()
    {
        var response = _httpClient.PostSync<IEnumerable<GetRoomRequestRoom>>($"{BaseApiUrl}/GetRoomsForHub");
        if (response == null) throw new Exception();

        return response;
    }
}
