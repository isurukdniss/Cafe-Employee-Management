using Autofac;
using CafeEmployeeManagement.Infrastructure.Persistence;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;
using CafeEmployeeManagement.Infrastructure.Repositories;
using CafeEmployeeManagement.Domain.Interfaces;

namespace CafeEmployeeManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static ContainerBuilder RegisterInfrastructureServices(this ContainerBuilder builder,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (connectionString == null)
            {
                throw new Exception(); // TODO: Handle exception properly
            }

            builder.RegisterInstance(connectionString)
                .As<string>()
                .Named<string>("DefaultConnection");

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseNpgsql(connectionString);
                return new ApplicationDbContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            var mediatRConfig = MediatRConfigurationBuilder
                .Create(typeof(DependencyInjection).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build();

            builder.RegisterMediatR(mediatRConfig);

            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CafeRepository>().As<ICafeRepository>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();


            return builder;
        }
    }
}
