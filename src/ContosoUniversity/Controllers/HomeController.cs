namespace ContosoUniversity.Controllers
{
    using Microsoft.AspNet.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "CU 2016 application description page.";

            return View();
        }
    }
}
