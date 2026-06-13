using Microsoft.AspNetCore.Mvc;

namespace CollegeNBU.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
