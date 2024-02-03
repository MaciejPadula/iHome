using iHome.Core.Services;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using iHome.Users.Api.Response;
using iHome.Users.Features.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.Users.Api;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IMediator _mediator;

    public UserController(IUserAccessor userAccessor, IMediator mediator)
        : base(userAccessor)
    {
        _mediator = mediator;
    }

    [HttpGet("GetUsers/{searchPhrase}")]
    public async Task<IActionResult> GetUsers(string searchPhrase)
    {
        var query = new GetUsersQuery { SearchPhrase = searchPhrase };
        await _mediator.HandleQueryAsync(query);

        return Ok(new GetUsersResponse
        {
            Users = query.Result
        });
    }
}
