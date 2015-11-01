namespace ContosoUniversity.Features.Student
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;
    using Models;

    public class _Controller : MediatorController
    {
        public async Task<IActionResult> Index(Index.Query query)
        {
            var model = await Mediator.SendAsync(query);

            return View(model);
        }
    }
}
