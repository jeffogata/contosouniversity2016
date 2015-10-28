namespace ContosoUniversity.Infrastructure
{
    using MediatR;
    using Microsoft.AspNet.Mvc;

    public abstract class MediatorController : Controller
    {
        // note:  must have a public setter to be set for [FromServices]
        [FromServices]
        public IMediator Mediator { get; set; }

        [NonAction]
        public virtual HttpNoContentResult HttpNoContent()
        {
            return new HttpNoContentResult();            
        }

        [NonAction]
        public virtual HttpCreatedResult HttpCreated()
        {
            return new HttpCreatedResult();
        }
    }
}
