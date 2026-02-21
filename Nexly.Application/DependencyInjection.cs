using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly));

            services.AddAutoMapper(cfg=> 
            {
                cfg.LicenseKey = config["Licences:MediatR"];
            },
                typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
