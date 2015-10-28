namespace ContosoUniversity.Infrastructure
{
    using Microsoft.AspNet.Http;
    using Microsoft.AspNet.Mvc;

    public class HttpCreatedResult : HttpStatusCodeResult
    {
        public HttpCreatedResult() : base(StatusCodes.Status201Created)
        {
        }
    }
}