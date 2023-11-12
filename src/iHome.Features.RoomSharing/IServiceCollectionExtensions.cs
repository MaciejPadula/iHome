using iHome.Features.RoomSharing;
using iHome.Features.RoomSharing.Service;
using iHome.Features.RoomSharing.Service.GetRoomUsers;
using iHome.Features.RoomSharing.Service.ShareRoom;
using iHome.Features.RoomSharing.Service.UnshareRoom;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomsList;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRoomSharing(this IServiceCollection services)
    {
        services.AddScoped<IAsyncCommandHandler<ShareRoomCommand>, ShareRoomCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<UnshareRoomCommand>, UnshareRoomCommandHandler>();
        services.AddScoped<IAsyncQueryHandler<GetRoomUsersQuery>, GetRoomUsersQueryHandler>();
        services.AddScoped<IRoomSharingService, RoomSharingService>();
        return services;
    }
}
