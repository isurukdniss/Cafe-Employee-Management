using Autofac;
using AutoMapper;
using CafeEmployeeManagement.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Serilog;
using System.Reflection;

namespace CafeEmployeeManagement.Application
{
    public static class DependencyInjection
    {
        public static ContainerBuilder RegisterApplicationServices(this ContainerBuilder builder)
        {
            var configuration = MediatRConfigurationBuilder
                .Create(typeof(DependencyInjection).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
            builder.RegisterMediatR(configuration);

            //builder.RegisterAssemblyTypes(typeof(CreateEmployeeCommandValidator).Assembly)
            //   .AsImplementedInterfaces()
            //   .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(UpdateEmployeeCommandValidator).Assembly)
            //   .AsImplementedInterfaces()
            //   .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(CreateCafeCommandValidator).Assembly)
            //   .AsImplementedInterfaces()
            //   .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(UpdateCafeCommandValidator).Assembly)
            //   .AsImplementedInterfaces()
            //   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .AsClosedTypesOf(typeof(IValidator<>))
               .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerLifetimeScope();

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console().CreateLogger();

            builder.Register(c => Log.Logger).As<ILogger>().SingleInstance();

            builder.Register(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(Assembly.GetExecutingAssembly());
                });

                return config.CreateMapper();
            }).As<IMapper>().SingleInstance();

            return builder;
        }
    }
}
