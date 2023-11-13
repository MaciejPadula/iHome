using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Schedules.Contract.Models;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public class DapperScheduleDeviceRepository : RepositoryBase, IScheduleDeviceRepository
{
    public DapperScheduleDeviceRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<Guid> Add(Guid scheduleId, Guid deviceId, string deviceData)
    {
        var id = Guid.NewGuid();
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[SchedulesDevices]
    (Id, DeviceId, ScheduleId, DeviceData)
VALUES
    (@Id, @DeviceId, @ScheduleId, @DeviceData)
        ", new
        {
            Id = id,
            DeviceId = deviceId,
            ScheduleId = scheduleId,
            DeviceData = deviceData
        });

        return id;
    }

    public async Task<int> CountByScheduleId(Guid scheduleId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<int>(@"
SELECT Count(Id)
FROM [maciejadmin].[SchedulesDevices]
WHERE ScheduleId = @ScheduleId
", new { ScheduleId = scheduleId });
    }

    public async Task<ScheduleDeviceModel?> GetByIdAndScheduleId(Guid deviceId, Guid scheduleId)
    {
        using var conn = GetDbConnection();

        return await conn.QuerySingleOrDefaultAsync<ScheduleDeviceModel?>(@$"
SELECT
    sd.Id as {nameof(ScheduleDeviceModel.Id)},
    d.Id as {nameof(ScheduleDeviceModel.DeviceId)},
    sd.ScheduleId as {nameof(ScheduleDeviceModel.ScheduleId)},
    d.Name as {nameof(ScheduleDeviceModel.Name)},
    d.Type as {nameof(ScheduleDeviceModel.Type)}
FROM [maciejadmin].[SchedulesDevices] sd
INNER JOIN [maciejadmin].[Devices] d
ON sd.DeviceId = d.Id
WHERE d.Id = @DeviceId AND sd.ScheduleId = @ScheduleId
", new { DeviceId = deviceId, ScheduleId = scheduleId });
    }

    public async Task<IEnumerable<ScheduleDeviceModel>> GetByScheduleId(Guid scheduleId)
    {
        using var conn = GetDbConnection();

        return await conn.QueryAsync<ScheduleDeviceModel>(@$"
SELECT
    sd.Id as {nameof(ScheduleDeviceModel.Id)},
    d.Id as {nameof(ScheduleDeviceModel.DeviceId)},
    sd.ScheduleId as {nameof(ScheduleDeviceModel.ScheduleId)},
    d.Name as {nameof(ScheduleDeviceModel.Name)},
    sd.DeviceData as {nameof(ScheduleDeviceModel.DeviceData)},
    d.Type as {nameof(ScheduleDeviceModel.Type)}
FROM [maciejadmin].[SchedulesDevices] sd
INNER JOIN [maciejadmin].[Devices] d
ON sd.DeviceId = d.Id
WHERE sd.ScheduleId = @ScheduleId
", new { ScheduleId = scheduleId });
    }

    public async Task Remove(Guid scheduleId, Guid deviceId)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[SchedulesDevices]
WHERE ScheduleId = @ScheduleId AND DeviceId = @DeviceId
        ", new { ScheduleId = scheduleId, DeviceId = deviceId });
    }

    public async Task Update(Guid scheduleId, Guid deviceId, string deviceData)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
UPDATE [maciejadmin].[SchedulesDevices]
SET DeviceData = @DeviceData
WHERE ScheduleId = @ScheduleId AND DeviceId = @DeviceId
        ", new { ScheduleId = scheduleId, DeviceId = deviceId, DeviceData = deviceData });
    }
}
