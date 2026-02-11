using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace BuildingBlocks.Infrastructure.Configuration;

public class BaseMediatorModule : Module
{
    private readonly Assembly[] _assembliesToScan;

    /// <summary>
    /// Constructor mặc định - sẽ scan tất cả assemblies đã load
    /// </summary>
    public BaseMediatorModule()
    {
        // Mặc định: scan tất cả assemblies trong AppDomain
        _assembliesToScan = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && a.GetName().Name?.StartsWith("BuildingBlocks") == false)
            .ToArray();
    }

    /// <summary>
    /// Constructor với assemblies cụ thể
    /// </summary>
    public BaseMediatorModule(params Assembly[] assembliesToScan)
    {
        _assembliesToScan = assembliesToScan ?? throw new ArgumentNullException(nameof(assembliesToScan));
    }

    protected override void Load(ContainerBuilder builder)
    {
        // Register ServiceProvider wrapper
        builder.RegisterType<ServiceProviderWrapper>()
            .As<IServiceProvider>()
            .InstancePerDependency()
            .IfNotRegistered(typeof(IServiceProvider));

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        var mediatorOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestHandler<>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>),
            typeof(IRequestPreProcessor<>),
            typeof(IStreamRequestHandler<,>),
            typeof(IRequestPostProcessor<,>),
            typeof(IRequestExceptionHandler<,,>),
            typeof(IRequestExceptionAction<,>),
        };

        builder.RegisterSource(new ScopedContravariantRegistrationSource(mediatorOpenTypes));

        if (_assembliesToScan.Any())
        {
            foreach (var mediatorOpenType in mediatorOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(_assembliesToScan)
                    .AsClosedTypesOf(mediatorOpenType)
                    .AsImplementedInterfaces()
                    .FindConstructorsWith(new AllConstructorFinder());
            }
        }

        builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));

        builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
    }

    private class ScopedContravariantRegistrationSource : IRegistrationSource
    {
        private readonly ContravariantRegistrationSource _source = new();
        private readonly List<Type>                      _types  = new();

        public ScopedContravariantRegistrationSource(params Type[] types)
        {
            ArgumentNullException.ThrowIfNull(types);

            if (!types.All(x => x.IsGenericTypeDefinition))
            {
                throw new ArgumentException("Supplied types should be generic type definitions");
            }

            _types.AddRange(types);
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var components = _source.RegistrationsFor(service, registrationAccessor);
            foreach (var c in components)
            {
                var defs = c.Target.Services
                    .OfType<TypedService>()
                    .Select(x => x.ServiceType.GetGenericTypeDefinition());

                if (defs.Any(_types.Contains))
                {
                    yield return c;
                }
            }
        }

        public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
    }
}