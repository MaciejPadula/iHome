using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models.Requests;
using iHome.Shared.Logic;

namespace iHome.Devices.ApiClient;

public class HttpDeviceManipulator : IDeviceManipulator
{
    private readonly JsonHttpClient _httpClient;
    private readonly ApiClientSettings _settings;

    private readonly string BaseApiUrl;
    private const string ApiSuffix = "api/Device";

    public HttpDeviceManipulator(JsonHttpClient httpClient, ApiClientSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        BaseApiUrl = settings.BaseApiUrl;
    }

    public Guid AddDevice(AddDeviceRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        return _httpClient.PostSync<Guid>($"{BaseApiUrl}{ApiSuffix}/AddDevice", request);
    }

    public void ChangeDeviceName(ChangeDeviceNameRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        _httpClient.PostSync($"{BaseApiUrl}{ApiSuffix}/ChangeDeviceName", request);
    }

    public void ChangeDeviceRoom(ChangeDeviceRoomRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        _httpClient.PostSync($"{BaseApiUrl}{ApiSuffix}/ChangeDeviceRoom", request);
    }

    public void RemoveDevice(RemoveDeviceRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        _httpClient.PostSync($"{BaseApiUrl}{ApiSuffix}/RemoveDevice", request);
    }

    public void SetDeviceData(SetDeviceDataRequest request)
    {
        _httpClient.SetBearerToken(_settings.Authorization);

        _httpClient.PostSync($"{BaseApiUrl}{ApiSuffix}/RemoveDevice", request);
    }
}
