namespace ContosoUniversity.Features.Department
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;

    [Route("api/department")]
    public class _Api : MediatorController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await Mediator.SendAsync(new Index.Query()));
        }
    }
}
