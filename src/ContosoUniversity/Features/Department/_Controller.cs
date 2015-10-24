namespace ContosoUniversity.Features.Department
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Framework.DependencyInjection;

    public class _Controller : Controller
    {
        private readonly IMediator _mediator;

        public _Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _mediator.SendAsync(new Index.Query());

            return View(model);
        }
        public ActionResult Create()
        {
            return View(new Create.Command());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command model)
        {
            await _mediator.SendAsync(model);

            return RedirectToAction("Index");
        }
    }
}
