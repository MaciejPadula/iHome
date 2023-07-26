using System.Collections.Generic;

namespace iHome.Microservices.UsersApi.Contract.Models.Request
{
    public class GetUsersByIdsRequest
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
