namespace ContosoUniversity.Infrastructure
{
    using Microsoft.AspNet.Http;
    using Microsoft.AspNet.Mvc;

    // MVC 6 does not declare this result class
    public class HttpNoContentResult : HttpStatusCodeResult
    {
        public HttpNoContentResult() : base(StatusCodes.Status204NoContent)
        {
        }
    }
}