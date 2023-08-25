using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Domain.Repositories;

namespace iHome.Microservices.Devices.Handlers;

public class DeviceDataHandler : IDeviceDataHandler
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IEnumerable<IDeviceDataRepository> _deviceDataRepositories;

    public DeviceDataHandler(IEnumerable<IDeviceDataRepository> deviceDataRepositories, IDeviceRepository deviceRepository)
    {
        _deviceDataRepositories = deviceDataRepositories;
        _deviceRepository = deviceRepository;
    }

    public async Task<GetDeviceDataResponse> GetDeviceData(GetDeviceDataRequest request)
    {
        var device = await _deviceRepository.GetByDeviceId(request.DeviceId)
            ?? throw new Exception();

        var tasks = _deviceDataRepositories.Select(repo => repo.GetDeviceData(device.MacAddress)).ToList();

        await Task.WhenAll(tasks);

        var result = tasks
            .Select(t => t.Result)
            .Where(r => !string.IsNullOrEmpty(r))
            .First();

        return new()
        {
            DeviceData = result
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
