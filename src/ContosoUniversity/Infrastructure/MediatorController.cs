namespace ContosoUniversity.Infrastructure
{
    using MediatR;
    using Microsoft.AspNet.Mvc;

    public abstract class MediatorController : Controller
    {
        // note:  must have a public setter to be set for [FromServices]
        [FromServices]
        public IMediator Mediator { get; set; }
    }
}
