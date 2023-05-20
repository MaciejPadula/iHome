using System.Collections.Generic;

namespace iHome.Microservices.Widgets.Contract.Models.Response
{
    public class GetWidgetsResponse
    {
        public IEnumerable<WidgetModel> Widgets { get; set; }
    }
}
