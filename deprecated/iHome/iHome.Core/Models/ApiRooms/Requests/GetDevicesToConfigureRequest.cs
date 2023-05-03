﻿namespace iHome.Models.Account.Rooms.Requests
{
    public class GetDevicesToConfigureRequest
    {
        public string Ip { get; set; }

        public GetDevicesToConfigureRequest(string ip)
        {
            Ip = ip;
        }
    }
}