using Cafe.Bll.Contracts.Servises.BotTg;
using Cafe.Bll.Contracts.Servises.Customer;
using Cafe.Bll.Contracts.Servises.Menu;
using Cafe.Bll.Contracts.Servises.Order;
using Cafe.Bll.Contracts.Servises.Table;
using Cafe.Bll.Infrastructure.Servises;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Bll.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static void AddBll(this IServiceCollection services)
        {

            services.AddSingleton<ICustomerServise, CustomerServise>();
            services.AddSingleton<IMenuServise, MenuServise>();
            services.AddSingleton<IOrderServise, OrderServise>();
            services.AddSingleton<ITableServise, TableServise>();
            services.AddSingleton<IBotTgService, BotTgServise>();
            services.AddAutoMapper(typeof(AppMappingProfile));
        }
    }
}
