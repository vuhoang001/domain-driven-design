using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;

namespace Item.Infrastructure.Configuration.DataAccess;

public class DataAccessSqlFactory(ISqlConnectionFactory sqlConnectionFactory)
    : IDesignTimeDbContextFactory<MasterDataContext>
{
    public DataAccessSqlFactory() : this(new SqlConnectionFactory(
                                             "Server=localhost;Database=DDD_Item;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;"))
    {
    }

    public MasterDataContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("MasterDataConnection") ??
            throw new InvalidOperationException();
        var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        var loggerFactory  = new SerilogLoggerFactory(serilogLogger);
        var optionsBuilder = new DbContextOptionsBuilder<MasterDataContext>();

        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypeIdValueConverterSelector>();
        return new MasterDataContext(optionsBuilder.Options, loggerFactory);
    }
}