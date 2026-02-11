using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.EventBus;
using Serilog;

namespace BuildingBlocks.Infrastructure.Configuration;

public class ModuleConfigurationBuilder(
    string moduleName,
    string connectionString,
    ILogger logger,
    IExecutionContextAccessor executionContextAccessor,
    IEventsBus eventsBus)
{
    private readonly ContainerBuilder          _containerBuilder         = new();
    private          string                    _connectionString         = connectionString;
    private          ILogger?                  _logger                   = logger;
    private          IExecutionContextAccessor _executionContextAccessor = executionContextAccessor;
    private          IEventsBus?               _eventsBus                = eventsBus;
    private          bool                      _enableQuartz             = true;
    private          bool                      _enableOutbox             = true;

    public ModuleConfigurationBuilder WithConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    public ModuleConfigurationBuilder WithLogger(ILogger logger)
    {
        _logger = logger.ForContext("Module", moduleName);
        return this;
    }

    public ModuleConfigurationBuilder WithExecutionContext(IExecutionContextAccessor accessor)
    {
        _executionContextAccessor = accessor;
        return this;
    }

    public ModuleConfigurationBuilder WithEventsBus(IEventsBus eventsBus)
    {
        _eventsBus = eventsBus;
        return this;
    }

    public ModuleConfigurationBuilder DisableQuartz()
    {
        _enableQuartz = false;
        return this;
    }

    public ModuleConfigurationBuilder DisableOutbox()
    {
        _enableOutbox = false;
        return this;
    }

    public ModuleConfigurationBuilder RegisterCommonModules()
    {
        if (_logger != null)
        {
            _containerBuilder.RegisterModule(new BaseLoggingModule(_logger));
        }

        if (!string.IsNullOrEmpty(_connectionString))
        {
            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(_logger);
            _containerBuilder.RegisterModule(new BaseDataAccessModule(_connectionString, loggerFactory));
        }

        // if (_eventsBus != null)
        // {
        //     _containerBuilder.RegisterModule(new BaseEventsBusModule(_eventsBus));
        // }

        _containerBuilder.RegisterModule(new BaseMediatorModule());

        return this;
    }

    // Cho phép module register thêm
    public ModuleConfigurationBuilder RegisterCustomModule(Module module)
    {
        _containerBuilder.RegisterModule(module);
        return this;
    }

    // Register specific component
    public ModuleConfigurationBuilder RegisterComponent<TInterface, TImplementation>()
        where TImplementation : TInterface
    {
        _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        return this;
    }

    public IContainer Build()
    {
        if (_executionContextAccessor != null)
        {
            _containerBuilder.RegisterInstance(_executionContextAccessor);
        }

        return _containerBuilder.Build();
    }

    public ContainerBuilder GetContainerBuilder() => _containerBuilder;
}