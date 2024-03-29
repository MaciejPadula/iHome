﻿using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Devices.Contract
{
    public interface IDeviceDataService
    {
        Task SetDeviceData(SetDeviceDataRequest request);
        Task<GetDeviceDataResponse> GetDeviceData(GetDeviceDataRequest request);
    }
}
