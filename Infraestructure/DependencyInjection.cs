using Application.Interfaces.Repositories;
using Infraestructure.Persistence;
using Infraestructure.Persistence.Interceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registramos el interceptor
            services.AddScoped<AuditoriaInterceptor>();

            // Registramos el DbContext con Npgsql
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditoriaInterceptor>();

                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(interceptor);
            });

            services.AddScoped<IProductoRepository, Repositories.ProductoRepository>();
            services.AddScoped<IVentaRepository, Repositories.VentaRepository>();

            return services;
        }
    }
}
