using iHome.Logic;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IUserAccessor _userAccessor;

    public BaseApiController(IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
    }
}
