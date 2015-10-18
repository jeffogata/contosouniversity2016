namespace ContosoUniversity.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Mvc.Razor;

    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // nothing to do here
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            return viewLocations.Select(x => x.Replace("/Views/", "/Features/"));
        }
    }
}