using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Infrastructure.Repositories;

namespace iHome.Microservices.Devices.Handlers;

public class DeviceDataHandler : IDeviceDataHandler
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly FirebaseDeviceDataRepository _firebaseDeviceDataRepository;
    private readonly IEnumerable<IDeviceDataRepository> _deviceDataRepositories;

    public DeviceDataHandler(IEnumerable<IDeviceDataRepository> deviceDataRepositories, IDeviceRepository deviceRepository, FirebaseDeviceDataRepository firebaseDeviceDataRepository)
    {
        _deviceDataRepositories = deviceDataRepositories;
        _deviceRepository = deviceRepository;
        _firebaseDeviceDataRepository = firebaseDeviceDataRepository;
    }

    public async Task<GetDeviceDataResponse> GetDeviceData(GetDeviceDataRequest request)
    {
        var device = await _deviceRepository.GetByDeviceId(request.DeviceId)
            ?? throw new Exception();

        return new()
        {
            DeviceData = await _firebaseDeviceDataRepository.GetDeviceData(
                device.MacAddress)
        };
    }

    public async Task SetDeviceData(SetDeviceDataRequest request)
    {
        var device = await _deviceRepository.GetByDeviceId(request.DeviceId)
            ?? throw new Exception();

        var tasks = _deviceDataRepositories.Select(repo => repo.SetDeviceData(
            device.MacAddress, request.Data));

        await Task.WhenAll(tasks);
    }
}
