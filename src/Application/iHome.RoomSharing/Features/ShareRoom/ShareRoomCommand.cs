﻿using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomSharing.Features.ShareRoom;

internal class ShareRoomCommand : ICommand
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; } = default!;
    public string CallerId { get; set; } = default!;
}
