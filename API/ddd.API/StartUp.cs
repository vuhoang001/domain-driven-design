using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.Email;
using ddd.API.Configuration.ExecutionContext;
using ddd.API.Extensions;
using ddd.API.Modules.Item;
using Item.Infrastructure.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace ddd.API;

public class StartUp
{
    private const    string         ConnectionString = "MeetingsConnectionString";
    private static   ILogger        _logger;
    private static   ILogger        _loggerForApi;
    private readonly IConfiguration _configuration;

    public StartUp(IWebHostEnvironment env)
    {
        ConfigurationLogger();
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json").AddUserSecrets<StartUp>()
            .AddEnvironmentVariables("ddd").Build();
    }

    private void ConfigurationLogger()
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
            .CreateLogger();

        _loggerForApi = _logger.ForContext("Module", "API");
        _loggerForApi.Information("Logger configured");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerDocumentation();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        var container = app.ApplicationServices.GetAutofacRoot();
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        InitializeModules(container);

        app.UseMiddleware<CorrelationMiddleware>();

        app.UseSwaggerDocumentation();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new ItemAutofacModule());
    }

    private void InitializeModules(ILifetimeScope container)
    {
        var httpContextAccessor      = container.Resolve<IHttpContextAccessor>();
        var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
        var emailsConfiguration =
            new EmailConfiguration(_configuration["EmailsConfiguration:FromEmail"] ??
                                   throw new InvalidOperationException());
        ItemStartUp.Initialize(ConnectionString, executionContextAccessor, _logger, emailsConfiguration, null);
    }
}