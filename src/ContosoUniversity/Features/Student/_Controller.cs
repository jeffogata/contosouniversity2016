namespace ContosoUniversity.Features.Student
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

        public ViewResult Index(Index.Query query)
        {
            var model = _mediator.Send(query);

            return View(model);
        }
    }
}
