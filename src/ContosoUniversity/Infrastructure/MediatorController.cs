namespace ContosoUniversity.Infrastructure
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNet.Mvc;
    using Models;

    public abstract class MediatorController<TEntity, TEntityView, TCreateView> : Controller
        where TEntity : Entity
    {
        // note:  must have a public setter to be set for [FromServices]
        [FromServices]
        public IMediator Mediator { get; set; }

        public async Task<IActionResult> Create()
        {
            return View(await Mediator.SendAsync(new CreateQuery<TEntity, TCreateView>()));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await Mediator.SendAsync(new DetailsQuery<TEntity, TEntityView>(id)));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await Mediator.SendAsync(new DetailsQuery<TEntity, TEntityView>(id)));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteCommand<TEntity> model)
        {
            await Mediator.SendAsync(model);
            return RedirectToAction("Index");
        }

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
