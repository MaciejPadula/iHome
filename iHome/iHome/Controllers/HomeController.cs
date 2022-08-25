using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
