using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;

namespace iHome.Devices.ApiClient;

public class HttpDeviceProvider : IDeviceProvider
{
    private readonly JsonHttpClient _httpClient;

    private readonly string BaseApiUrl = "/api/Device";

    public HttpDeviceProvider(JsonHttpClient httpClient, ApiClientSettings settings)
    {
        _httpClient = httpClient;
        BaseApiUrl = settings.BaseApiUrl;
    }

    public string GetDeviceData(Guid deviceId)
    {
        var response = _httpClient.GetSync<string>($"{BaseApiUrl}/GetDeviceData/{deviceId}");
        if (string.IsNullOrEmpty(response)) throw new Exception();

        return response;
    }

    public IEnumerable<Device> GetDevices(Guid roomId)
    {
        var response = _httpClient.GetSync<IEnumerable<Device>>($"{BaseApiUrl}/GetDeviceData/{roomId}");
        if(response == null) throw new Exception();

        return response;
    }
}
