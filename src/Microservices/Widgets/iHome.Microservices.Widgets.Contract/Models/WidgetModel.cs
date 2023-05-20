using System;

namespace iHome.Microservices.Widgets.Contract.Models
{
    public class WidgetModel
    {
        public Guid Id { get; set; }
        public WidgetType WidgetType { get; set; }
        public Guid RoomId { get; set; }
        public bool ShowBorder { get; set; }
        public int MaxNumberOfDevices { get; set; }
    }
}