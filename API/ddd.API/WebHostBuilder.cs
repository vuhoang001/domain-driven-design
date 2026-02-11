using Autofac.Extensions.DependencyInjection;

namespace ddd.API;

public static class WebHostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<StartUp>(); });
    }
}