using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace Item.Infrastructure.Configuration.DataAccess;

public class DataAccessModule(string connectionString, ILoggerFactory loggerFactory) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SqlConnectionFactory>()
            .As<ISqlConnectionFactory>()
            .WithParameter("connectionString", connectionString)
            .InstancePerLifetimeScope();

        builder.Register(c =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<ItemContext>();
                dbContextOptions.UseSqlServer(connectionString);

                dbContextOptions.ReplaceService<IValueConverterSelector, StronglyTypeIdValueConverterSelector>();
                return new ItemContext(dbContextOptions.Options, loggerFactory);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();

        var infrastructureAssembly = typeof(ItemContext).Assembly;

        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());
    }
}