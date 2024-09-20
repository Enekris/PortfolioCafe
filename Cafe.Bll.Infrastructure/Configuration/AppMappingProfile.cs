using AutoMapper;
using Cafe.Bll.Contracts.Servises.BotTg.Models;
using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Order.Models;
using Cafe.Bll.Contracts.Servises.Table.Models;
using Cafe.Dal.Contracts.Repositories.Bot.Models;
using Cafe.Dal.Contracts.Repositories.Customer.Models;
using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Contracts.Repositories.Order.Models;
using Cafe.Dal.Contracts.Repositories.Table.Models;

namespace Cafe.Bll.Infrastructure.Configuration
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CustomerDb, CustomerDto>();
            CreateMap<OrderDb, OrderDto>();//.ReverseMap(); 
            CreateMap<DishDb, DishDto>();
            CreateMap<TableDb, TableDto>();
            CreateMap<UserDataTgDb, UserDataTgDto>();

        }


    }
}
