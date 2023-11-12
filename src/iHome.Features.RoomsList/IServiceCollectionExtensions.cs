using iHome.Features.RoomsList.Service;
using iHome.Features.RoomsList.Service.AddRoom;
using iHome.Features.RoomsList.Service.GetUserRooms;
using iHome.Features.RoomsList.Service.RemoveRoom;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomsList;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRoomsList(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<GetUserRoomsQuery>, GetUserRoomsQueryHandler>();
        services.AddScoped<IAsyncCommandHandler<AddRoomCommand>, AddRoomCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<RemoveRoomCommand>, RemoveRoomCommandHandler>();
        services.AddScoped<IRoomsListService, RoomsListService>();
        return services;
    }
}
