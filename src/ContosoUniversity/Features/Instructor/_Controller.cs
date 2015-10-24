using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Features.Instructor
{
    using MediatR;
    using Microsoft.AspNet.Mvc;

    public class _Controller : Controller
    {
        private readonly IMediator _mediator;

        public _Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index(Index.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }
    }

}
