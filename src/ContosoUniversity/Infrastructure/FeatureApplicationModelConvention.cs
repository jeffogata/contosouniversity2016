namespace ContosoUniversity.Infrastructure
{
    using System.Linq;
    using Microsoft.AspNet.Mvc.ApplicationModels;

    public class FeatureApplicationModelConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(x => x.ControllerName == "_"))
            {
                var name = controller.ControllerType.AsType()?.Namespace?.Split().Last();

                if (name != null)
                {
                    controller.ControllerName = name;
                }
            }
        }
    }
}