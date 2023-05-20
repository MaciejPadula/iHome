using Dapper;
using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Devices.Infrastructure.Repositories;

public class DapperDeviceRepository : IDeviceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperDeviceRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> Add(string name, string macAddress, DeviceType type, Guid roomId)
    {
        using var conn = _connectionFactory.GetConnection();
        var id = Guid.NewGuid();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[Devices]
    (Id, Name, MacAddress, Type, RoomId)
VALUES
    (@Id, @Name, @MacAddress, @Type, @RoomId)
        ", new { Id = id, Name = name, MacAddress = macAddress, Type = type, RoomId = roomId });

        return id;
    }

    public async Task ChangeRoom(Guid deviceId, Guid roomId)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
UPDATE [maciejadmin].[Devices]
SET RoomId = @RoomId
WHERE Id = @Id
        ", new { Id = deviceId });
    }

    public async Task<DeviceModel> GetByDeviceId(Guid deviceId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QuerySingleAsync<DeviceModel>(@$"
SELECT
    Id as {nameof(DeviceModel.Id)},
    Name as {nameof(DeviceModel.Name)},
    Type as {nameof(DeviceModel.Type)},
    MacAddress as {nameof(DeviceModel.MacAddress)},
    RoomId as {nameof(DeviceModel.RoomId)}
FROM [maciejadmin].[Devices]
WHERE Id = @Id
", new { Id = deviceId });
    }

    public async Task<IEnumerable<DeviceModel>> GetByRoomId(Guid roomId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QueryAsync<DeviceModel>(@$"
SELECT
    Id as {nameof(DeviceModel.Id)},
    Name as {nameof(DeviceModel.Name)},
    Type as {nameof(DeviceModel.Type)},
    MacAddress as {nameof(DeviceModel.MacAddress)},
    RoomId as {nameof(DeviceModel.RoomId)}
FROM [maciejadmin].[Devices]
WHERE RoomId = @RoomId
", new { RoomId = roomId });
    }

    public async Task<IEnumerable<DeviceModel>> GetByUserId(string userId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QueryAsync<DeviceModel>(@$"
SELECT
    d.Id as {nameof(DeviceModel.Id)},
    d.Name as {nameof(DeviceModel.Name)},
    d.Type as {nameof(DeviceModel.Type)},
    d.MacAddress as {nameof(DeviceModel.MacAddress)},
    d.RoomId as {nameof(DeviceModel.RoomId)}
FROM [maciejadmin].[Devices] d
INNER JOIN [maciejadmin].[Rooms] r
ON  d.RoomId = r.Id
WHERE r.UserId = @UserId
", new { UserId = userId });
    }

    public async Task Remove(Guid deviceId)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[Devices]
WHERE Id = @Id
        ", new { Id = deviceId });
    }

    public async Task Rename(Guid deviceId, string name)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
UPDATE [maciejadmin].[Devices]
SET Name = @Name
WHERE Id = @Id
        ", new { Id = deviceId, Name = name });
    }
}
