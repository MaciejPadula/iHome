﻿namespace iHome.Models;

public class InsertDeviceRequest
{
    public required Guid WidgetId { get; set; }
    public required Guid DeviceId { get; set; }
}
