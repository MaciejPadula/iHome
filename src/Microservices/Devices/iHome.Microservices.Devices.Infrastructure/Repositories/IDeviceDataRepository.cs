namespace iHome.Microservices.Devices.Infrastructure.Repositories;

public interface IDeviceDataRepository
{
    Task<string> GetDeviceData(string macAddess);
    Task SetDeviceData(string macAddess, string data);
}
