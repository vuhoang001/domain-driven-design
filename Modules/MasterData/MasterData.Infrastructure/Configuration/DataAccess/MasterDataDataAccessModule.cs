using Autofac;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasterData.Infrastructure.Configuration.DataAccess;

internal class MasterDataDataAccessModule(string connectionString, ILoggerFactory loggerFactory) : Module
{
    private readonly string _connectionString =
        connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    private readonly ILoggerFactory _loggerFactory =
        loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

    protected override void Load(ContainerBuilder builder)
    {
        // Register MasterDataContext
        builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<MasterDataContext>();

                optionsBuilder
                    .UseSqlServer(_connectionString)
                    .UseLoggerFactory(_loggerFactory)
                    .EnableSensitiveDataLogging(); // Chỉ dùng trong Development

                return new MasterDataContext(optionsBuilder.Options, _loggerFactory);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();

        // Register UnitOfWork
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        // Auto-register tất cả repositories
        var infrastructureAssembly = typeof(MasterDataContext).Assembly;
        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());
    }
}