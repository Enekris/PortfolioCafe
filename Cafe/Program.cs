using Cafe.Bll.Infrastructure.Configuration;
using Cafe.Dal.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Cafe
{
    internal class Program
    {
        private static IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices(services =>
            {
                services.AddDal();
                services.AddBll();
                // services.AddStartup();
                services.AddStartupTg();
            });
            return builder;
        }

        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder().Build();
            StartupTg app = host.Services.GetRequiredService<StartupTg>();

            await app.Main();

        }
    }
}