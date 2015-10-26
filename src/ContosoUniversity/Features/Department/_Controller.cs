namespace ContosoUniversity.Features.Department
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;

    public class _Controller : MediatorController
    {
        public async Task<IActionResult> Index()
        {
            var model = await Mediator.SendAsync(new Index.Query());

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await Mediator.SendAsync(new Details.Query(id));

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await Mediator.SendAsync(new Create.Query());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command model)
        {
            await Mediator.SendAsync(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await Mediator.SendAsync(new Details.Query(id));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Delete.Command model)
        {
            await Mediator.SendAsync(model);

            return RedirectToAction("Index");
        }
    }
}