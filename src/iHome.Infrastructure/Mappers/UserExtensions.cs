using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Model;

namespace iHome.Infrastructure.Mappers;
internal static class UserExtensions
{
    internal static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}
