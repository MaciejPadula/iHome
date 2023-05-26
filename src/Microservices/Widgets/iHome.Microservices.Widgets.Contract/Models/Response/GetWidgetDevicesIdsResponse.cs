using System.Collections.Generic;
using System;

namespace iHome.Microservices.Widgets.Contract.Models.Response
{
    public class GetWidgetDevicesIdsResponse
    {
        public IEnumerable<Guid> DevicesIds { get; set; }
    }
}
