using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Configuration;

public class BaseDataAccessModule(string connectionString, ILoggerFactory loggerFactory) : Module
{
    private readonly string         _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    private readonly ILoggerFactory _loggerFactory    = loggerFactory    ?? throw new ArgumentNullException(nameof(loggerFactory));

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DbContext>();

                optionsBuilder
                    .UseSqlServer(_connectionString)
                    .UseLoggerFactory(_loggerFactory)
                    .EnableSensitiveDataLogging();

                return optionsBuilder.Options;
            }).As<DbContextOptions>()
            .InstancePerLifetimeScope();

        builder.Register(c => new SqlConnectionFactory(_connectionString))
            .As<ISqlConnectionFactory>()
            .InstancePerLifetimeScope();
    }
}

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public System.Data.SqlClient.SqlConnection GetOpenConnection()
    {
        var connection = new System.Data.SqlClient.SqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}

public interface ISqlConnectionFactory
{
    System.Data.SqlClient.SqlConnection GetOpenConnection();
}