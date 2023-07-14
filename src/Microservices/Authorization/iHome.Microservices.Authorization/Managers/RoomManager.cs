using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Infrastructure.Repositories;

namespace iHome.Microservices.Authorization.Managers;

public interface IRoomManager
{
    Task<bool> CanRead(RoomAuthRequest request);
    Task<bool> CanWrite(RoomAuthRequest request);
}

public class RoomManager : IRoomManager
{
    private readonly IRoomRepository _roomRepository;

    public RoomManager(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public Task<bool> CanRead(RoomAuthRequest request) =>
        _roomRepository.UserHasReadAccess(request.RoomId, request.UserId);

    public Task<bool> CanWrite(RoomAuthRequest request) =>
        _roomRepository.UserHasWriteAccess(request.RoomId, request.UserId);
}
