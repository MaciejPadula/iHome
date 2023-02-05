using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models.Requests;

namespace iHome.Devices.ApiClient;

public class HttpDeviceManipulator : IDeviceManipulator
{
    private readonly JsonHttpClient _httpClient;

    private readonly string BaseApiUrl = "/api/Device";

    public HttpDeviceManipulator(JsonHttpClient httpClient, ApiClientSettings settings)
    {
        _httpClient = httpClient;
        BaseApiUrl = settings.BaseApiUrl;
    }

    public Guid AddDevice(AddDeviceRequest request)
    {
        return _httpClient.PostSync<Guid>($"{BaseApiUrl}/AddDevice", request);
    }

    public void ChangeDeviceName(ChangeDeviceNameRequest request)
    {
        _httpClient.PostSync($"{BaseApiUrl}/ChangeDeviceName", request);
    }

    public void ChangeDeviceRoom(ChangeDeviceRoomRequest request)
    {
        _httpClient.PostSync($"{BaseApiUrl}/ChangeDeviceRoom", request);
    }

    public void RemoveDevice(RemoveDeviceRequest request)
    {
        _httpClient.PostSync($"{BaseApiUrl}/RemoveDevice", request);
    }

    public void SetDeviceData(SetDeviceDataRequest request)
    {
        _httpClient.PostSync($"{BaseApiUrl}/RemoveDevice", request);
    }
}
