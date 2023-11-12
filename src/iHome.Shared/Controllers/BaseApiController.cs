using iHome.Shared.Logic;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Shared.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IUserAccessor _userAccessor;

    public BaseApiController(IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
    }
}
