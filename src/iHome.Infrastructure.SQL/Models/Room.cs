﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Infrastructure.SQL.Models;

[Table("Rooms", Schema = "maciejadmin")]
public class Room
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string UserId { get; init; }

    public virtual Collection<Widget> Widgets { get; init; } = new();
    public virtual Collection<Device> Devices { get; init; } = new();
    public virtual Collection<UserRoom> UsersRooms { get; init; } = new();
}