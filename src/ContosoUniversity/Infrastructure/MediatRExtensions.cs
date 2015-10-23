namespace ContosoUniversity.Infrastructure
{
    using System.Linq;
    using System.Reflection;
    using MediatR;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.DependencyInjection;

    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddScoped<IMediator>(
                servicesProvider => new Mediator(servicesProvider.GetService, servicesProvider.GetServices));

            var libraryManager = services.BuildServiceProvider().GetService<ILibraryManager>();

            var assemblies = libraryManager
                .GetReferencingLibraries("MediatR")
                .Distinct()
                .SelectMany(l => l.Assemblies)
                .Select(Assembly.Load);

            var handlerTypes = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(typeInfo => typeInfo.GetInterfaces().Any(x =>
                    x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IAsyncRequestHandler<,>)));

            foreach (var type in handlerTypes)
            {
                var interfaceType =
                    type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof (IAsyncRequestHandler<,>));
                services.AddScoped(interfaceType, type);
            }

            return services;
        }
    }
}