using Microsoft.Extensions.DependencyInjection;

namespace Cafe
{
    public static class DependencyInjection
    {
        public static void AddStartup(this IServiceCollection services)
        {
            services.AddSingleton<Startup>();
        }

        public static void AddStartupTg(this IServiceCollection services)
        {
            services.AddSingleton<StartupTg>();
        }
    }
}
