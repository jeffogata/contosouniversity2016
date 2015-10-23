namespace ContosoUniversity.Infrastructure
{
    using System.Collections.Generic;
    using ContosoUniversity.Features.Course;
    using MediatR;
    using Microsoft.Framework.DependencyInjection;

    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddScoped<IMediator>(servicesProvider => new Mediator(servicesProvider.GetService, servicesProvider.GetServices));

            services.AddScoped(typeof (IAsyncRequestHandler<Index.Query, Index.Result>), typeof (Index.Handler));
            services.AddScoped(
                typeof (IAsyncRequestHandler<Features.Department.Index.Query, List<Features.Department.Index.Model>>),
                typeof (Features.Department.Index.QueryHandler));

            return services;
        }
    }
}