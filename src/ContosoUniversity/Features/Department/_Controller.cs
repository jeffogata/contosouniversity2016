namespace ContosoUniversity.Features.Department
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;
    using Models;

    public class _Controller : MediatorController<Department, Details.QueryModel, DepartmentCreateQueryResponse>
    {
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.SendAsync(new Index.Query()));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await Mediator.SendAsync(new Details.Query(id)));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await Mediator.SendAsync(new Details.Query(id)));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command model)
        {
            await Mediator.SendAsync(model);
            return RedirectToAction("Index");
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
    }
}