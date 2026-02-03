using Autofac;
using Serilog;

namespace Item.Infrastructure.Configuration.Logging;

public class LoggingModule(ILogger logger) : Module
{
   protected override void Load(ContainerBuilder builder)
   {
      builder.RegisterInstance(logger)
         .As<ILogger>()
         .SingleInstance();
   } 
}