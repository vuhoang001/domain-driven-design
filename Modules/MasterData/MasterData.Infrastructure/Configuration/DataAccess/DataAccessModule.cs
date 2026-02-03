using Autofac;
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

        builder.Register(_ =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<MasterDataContext>();
                dbContextOptions.UseSqlServer(connectionString);

                dbContextOptions.ReplaceService<IValueConverterSelector, StronglyTypeIdValueConverterSelector>();
                return new MasterDataContext(dbContextOptions.Options, loggerFactory);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();

        var infrastructureAssembly = typeof(MasterDataContext).Assembly;

        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());
    }
}