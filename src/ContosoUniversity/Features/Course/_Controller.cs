namespace ContosoUniversity.Features.Course
{
    using Microsoft.AspNet.Mvc;

    public class _Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
