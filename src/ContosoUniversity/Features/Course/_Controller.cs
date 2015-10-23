namespace ContosoUniversity.Features.Course
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

        public async Task<IActionResult> Index(Index.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }
    }
}
