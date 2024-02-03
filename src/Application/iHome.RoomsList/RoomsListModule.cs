using iHome.Microservices.RoomsManagement.Contract;
using iHome.RoomsList.Features.AddRoom;
using iHome.RoomsList.Features.GetUserRooms;
using iHome.RoomsList.Features.RemoveRoom;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;
using Web.Infrastructure.Cqrs.Mediator.Query;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.RoomsList;

public static class RoomsListModule
{
    public static IServiceCollection AddRoomsListModule(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<GetUserRoomsQuery>, GetUserRoomsQueryHandler>();
        services.AddScoped<IAsyncCommandHandler<AddRoomCommand>, AddRoomCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<RemoveRoomCommand>, RemoveRoomCommandHandler>();

        services.AddMicroserviceClient<IRoomManagementService>();
        return services;
    }
}
