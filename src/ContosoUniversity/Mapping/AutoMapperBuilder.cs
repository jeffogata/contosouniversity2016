﻿namespace ContosoUniversity.Mapping
{
    using System;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Microsoft.AspNet.Builder;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.DependencyInjection;

    public static class AutoMapperBuilder
    {
        public static void UseAutoMapper(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var libraryManager = services.GetService<ILibraryManager>();

            var assemblies = libraryManager
                .GetReferencingLibraries("AutoMapper")
                .Distinct()
                .SelectMany(l => l.Assemblies)
                .Select(Assembly.Load);

            var profiles = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(typeInfo => typeInfo.IsSubclassOf(typeof (Profile)))
                .Select(t => (Profile) Activator.CreateInstance(t));

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }

                cfg.ConstructServicesUsing(services.GetService);
            });
        }
    }
}