using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Mapping
{
    using Microsoft.Framework.DependencyInjection;

    public static class AutoMapperServiceCollectionExtensions
    {
        public static IAutoMapperBuilder AddAutoMapper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return new AutoMapperBuilder(services);
        }
    }

    public interface IAutoMapperBuilder
    {
        IServiceCollection Services { get; }
    }

    public class AutoMapperBuilder : IAutoMapperBuilder
    {
        private readonly IServiceCollection _services;

        public AutoMapperBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            _services = services;
        }

        public IServiceCollection Services
        {
            get
            {
                return _services;
            }
        }
    }
}
