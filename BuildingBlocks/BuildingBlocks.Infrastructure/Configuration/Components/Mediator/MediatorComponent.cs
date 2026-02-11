namespace BuildingBlocks.Infrastructure.Configuration.Components.Mediator;

    public class MediatorComponent : IModuleComponent
    {
        private readonly Assembly _assembly;

        public MediatorComponent(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string Name => "Mediator";

        public void Validate()
        {
            if (_assembly == null)
                throw new InvalidOperationException("Mediator assembly not provided.");
        }

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
