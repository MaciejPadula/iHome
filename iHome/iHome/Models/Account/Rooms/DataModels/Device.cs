﻿namespace iHome.Models.DataModels
{
    public class Device
    {
        public string deviceId { get; set; } = "";
        public int deviceType { get; set; } = 0;
        public string deviceName { get; set; } = "";
        public string deviceData { get; set; } = "";
        public int roomId { get; set; } = 0;
    }
}
