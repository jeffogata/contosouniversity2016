namespace ContosoUniversity.Features.Course
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;

    // todo: would be nice to have the route determined by convention
    [Route("api/course")]
    public class _Api : MediatorController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await Mediator.SendAsync(new Index.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await Mediator.SendAsync(new Details.Query(id));

            if (course == null)
            {
                return HttpNotFound();
            }

            return new JsonResult(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ApiDelete(int id)
        {
            await Mediator.SendAsync(new Delete.Command { Id = id });
            return HttpNoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command model)
        {
            if (model == null)
            {
                return HttpBadRequest();
            }

            await Mediator.SendAsync(model);
            return HttpCreated();
        }

        [HttpPut("{id?}")]
        public async Task<IActionResult> Edit(int? id, [FromBody] Edit.Command model)
        {
            if (model == null || id == null)
            {
                return HttpBadRequest();
            }

            model.Id = id.Value;

            var result = await Mediator.SendAsync(model);

            if (result == 0)
            {
                return HttpNotFound();
            }

            return HttpNoContent();
        }
    }
}
