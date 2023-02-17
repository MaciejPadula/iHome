using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.Devices.Contract.Models.Requests;
using iHome.Shared.Logic;

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

    public string GetDeviceData(GetDeviceDataRequest request)
    {
        var response = _httpClient.PostSync<string>($"{BaseApiUrl}/GetDeviceData", request);
        if (string.IsNullOrEmpty(response)) throw new Exception();

        return response;
    }

    public IEnumerable<Device> GetDevices(GetDevicesRequest request)
    {
        var response = _httpClient.PostSync<IEnumerable<Device>>($"{BaseApiUrl}/GetDeviceData", request);
        if(response == null) throw new Exception();

        return response;
    }
}
