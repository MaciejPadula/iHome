using iHome.Shared.Logic;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Shared.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly IUserAccessor _userAccessor;

    public BaseApiController(IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
    }
}
