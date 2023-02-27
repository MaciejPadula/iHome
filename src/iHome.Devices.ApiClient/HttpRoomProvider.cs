using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.Shared.Logic;

namespace iHome.Devices.ApiClient;

public class HttpRoomProvider : IRoomProvider
{
    private readonly JsonHttpClient _httpClient;
    private readonly ApiClientSettings _settings;

    private readonly string BaseApiUrl;
    private const string ApiSuffix = "api/Room";

    public HttpRoomProvider(JsonHttpClient httpClient, ApiClientSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        BaseApiUrl = settings.BaseApiUrl;
    }

    public IEnumerable<GetRoomRequestRoom> GetRoomsForHub()
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        var response = _httpClient.PostSync<IEnumerable<GetRoomRequestRoom>>($"{BaseApiUrl}{ApiSuffix}/GetRoomsForHub");
        if (response == null) throw new Exception();

        return response;
    }
}
