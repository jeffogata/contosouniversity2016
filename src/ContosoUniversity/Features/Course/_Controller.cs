namespace ContosoUniversity.Features.Course
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;
    using Models;

    public class _Controller : MediatorController<Course, Details.QueryModel, CourseCreateQueryResponse>
    {
        public async Task<IActionResult> Index(Index.Query query)
        {
            return View(await Mediator.SendAsync(query));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await Mediator.SendAsync(new Details.Query(id)));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await Mediator.SendAsync(new Details.Query(id)));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await Mediator.SendAsync(new Edit.Query(id)));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command model)
        {
            await Mediator.SendAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command model)
        {
            await Mediator.SendAsync(model);
            return RedirectToAction("Index");
        }
    }
}