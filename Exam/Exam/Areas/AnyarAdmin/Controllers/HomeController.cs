using Microsoft.AspNetCore.Mvc;

namespace Exam.Areas.AnyarAdmin.Controllers
{
    public class HomeController : Controller
    {
        [Area("AnyarAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
