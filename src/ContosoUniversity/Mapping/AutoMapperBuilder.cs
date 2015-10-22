using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Mapping
{
    using System.Reflection;
    using AutoMapper;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.DependencyInjection;

    public class AutoMapperBuilder
    {
        public AutoMapperBuilder()
        {
        }

        public AutoMapperBuilder AddProfiles(IServiceProvider serviceProvider, ILibraryManager libraryManager)
        {
            Mapper.Initialize(cfg =>
            {
                var assemblies = libraryManager.GetReferencingLibraries("AutoMapper")
                    .Distinct()
                    .SelectMany(l => l.Assemblies)
                    .Select(a => Assembly.Load(a));

                var profiles = assemblies.SelectMany(a => a.DefinedTypes)
                    .Where(typeInfo => typeInfo.IsSubclassOf(typeof (Profile)));

                foreach (var profile in profiles)
                {
                    //cfg.AddProfile(profile);
                }

                cfg.ConstructServicesUsing(serviceProvider.GetService);
            });

            return this;
        }
    }
}
