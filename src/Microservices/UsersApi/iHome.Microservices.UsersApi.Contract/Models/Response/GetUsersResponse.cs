using System.Collections.Generic;

namespace iHome.Microservices.UsersApi.Contract.Models.Response
{
    public class GetUsersResponse
    {
        public IEnumerable<User> Users { get; set; }
    }
}
