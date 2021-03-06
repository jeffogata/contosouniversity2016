﻿namespace ContosoUniversity.Infrastructure
{
    using System.Linq;
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;

    public static class MediatorExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddScoped<IMediator>(
                servicesProvider => new Mediator(servicesProvider.GetService, servicesProvider.GetServices));

            var libraryManager = services.BuildServiceProvider().GetService<ILibraryManager>();

            var assemblies = libraryManager
                .GetReferencingLibraries("MediatR")
                .Distinct()
                .SelectMany(l => l.Assemblies)
                .Select(Assembly.Load);

            var asyncHandlerTypes = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(typeInfo => !typeInfo.IsAbstract && typeInfo.GetInterfaces().Any(x =>
                    x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IAsyncRequestHandler<,>)));

            foreach (var type in asyncHandlerTypes)
            {
                var interfaceType =
                    type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof (IAsyncRequestHandler<,>));
                services.AddScoped(interfaceType, type);
            }

            var handlerTypes = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(typeInfo => !typeInfo.IsAbstract && typeInfo.GetInterfaces().Any(x =>
                    x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IRequestHandler<,>)));

            foreach (var type in handlerTypes)
            {
                var interfaceType =
                    type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof (IRequestHandler<,>));
                services.AddScoped(interfaceType, type);
            }

            return services;
        }
    }
}