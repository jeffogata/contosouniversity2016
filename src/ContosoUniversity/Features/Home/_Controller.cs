namespace ContosoUniversity.Features.Home
{
    using Microsoft.AspNet.Mvc;

    public class _Controller : Controller
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
