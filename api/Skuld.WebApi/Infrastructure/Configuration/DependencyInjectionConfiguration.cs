using Microsoft.Extensions.DependencyInjection;
using Skuld.Data.UnitOfWork;
using Skuld.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skuld.WebApi.Infrastructure.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddCustomDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<UnitOfWork>()
                .AddScoped<UserService>();
            
            return services;
        }
    }
}
