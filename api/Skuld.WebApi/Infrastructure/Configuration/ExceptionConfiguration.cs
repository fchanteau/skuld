using Microsoft.AspNetCore.Builder;
using Skuld.WebApi.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skuld.WebApi.Infrastructure.Configuration
{
    public static class ExceptionConfiguration
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
