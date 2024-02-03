using iHome.Microservices.RoomsManagement.Contract;
using iHome.RoomSharing.Features.GetRoomUsers;
using iHome.RoomSharing.Features.ShareRoom;
using iHome.RoomSharing.Features.UnshareRoom;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;
using Web.Infrastructure.Cqrs.Mediator.Query;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.RoomSharing;

public static class RoomSharingModule
{
    public static IServiceCollection AddRoomSharing(this IServiceCollection services)
    {
        services.AddScoped<IAsyncCommandHandler<ShareRoomCommand>, ShareRoomCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<UnshareRoomCommand>, UnshareRoomCommandHandler>();
        services.AddScoped<IAsyncQueryHandler<GetRoomUsersQuery>, GetRoomUsersQueryHandler>();
        services.AddMicroserviceClient<IRoomSharingService>();
        return services;
    }
}
