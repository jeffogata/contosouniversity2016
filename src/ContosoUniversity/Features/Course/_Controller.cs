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

        public _Controller(IServiceProvider services)
        {
            _mediator = new Mediator(services.GetService, services.GetServices);
        }

        public async Task<IActionResult> Index(Index.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }
    }
}
