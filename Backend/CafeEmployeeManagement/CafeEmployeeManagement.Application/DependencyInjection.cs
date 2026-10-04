using CafeEmployeeManagement.Application.Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace CafeEmployeeManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.AddValidatorsFromAssembly(assembly);

            services.AddAutoMapper(cfg => { }, assembly);

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console().CreateLogger();

            services.AddSingleton(Log.Logger);

            return services;
        }
    }
}
