using iHome.Microservices.RoomsManagement.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Model;

namespace iHome.Infrastructure.Mappers;

internal static class RoomModelExtensions
{
    internal static RoomDto ToDto(this RoomModel room, User user)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            User = user.ToDto(),
        };
    }
}
