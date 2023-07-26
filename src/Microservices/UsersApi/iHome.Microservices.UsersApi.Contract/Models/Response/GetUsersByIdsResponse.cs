using System.Collections.Generic;

namespace iHome.Microservices.UsersApi.Contract.Models.Response
{
    public class GetUsersByIdsResponse
    {
        public Dictionary<string, User> Users { get; set; }
    }
}
