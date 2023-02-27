using iHome.Devices.Contract.Models;

namespace iHome.Devices.Contract.Interfaces;

public interface IRoomProvider
{
    IEnumerable<GetRoomRequestRoom> GetRoomsForHub();
}
