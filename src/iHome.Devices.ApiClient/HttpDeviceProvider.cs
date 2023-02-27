using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.Devices.Contract.Models.Requests;
using iHome.Shared.Logic;

namespace iHome.Devices.ApiClient;

public class HttpDeviceProvider : IDeviceProvider
{
    private readonly JsonHttpClient _httpClient;
    private readonly ApiClientSettings _settings;

    private readonly string BaseApiUrl;
    private const string ApiSuffix = "api/Device";

    public HttpDeviceProvider(JsonHttpClient httpClient, ApiClientSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        BaseApiUrl = settings.BaseApiUrl;
    }

    public string GetDeviceData(GetDeviceDataRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        var response = _httpClient.PostSync<string>($"{BaseApiUrl}{ApiSuffix}/GetDeviceData", request);
        if (string.IsNullOrEmpty(response)) throw new Exception();

        return response;
    }

    public IEnumerable<Device> GetDevices(GetDevicesRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        var response = _httpClient.PostSync<IEnumerable<Device>>($"{BaseApiUrl}{ApiSuffix}/GetDeviceData", request);
        if (response == null) throw new Exception();

        return response;
    }
}
