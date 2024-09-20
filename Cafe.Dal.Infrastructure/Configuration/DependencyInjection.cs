using Cafe.Dal.Contracts.Repositories.Bot;
using Cafe.Dal.Contracts.Repositories.Customer;
using Cafe.Dal.Contracts.Repositories.Dish;
using Cafe.Dal.Contracts.Repositories.Order;
using Cafe.Dal.Contracts.Repositories.Table;
using Cafe.Dal.Infrastructure.RepositoriesEF;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Dal.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerRepositoty, CustomerRepository>();
            services.AddSingleton<IDishRepository, DishRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<ITableRepository, TableRepository>();
            services.AddSingleton<IBotTgRepositoty, BotTgRepositoty>();
        }
    }
}
