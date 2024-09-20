using AutoMapper;
using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Order;
using Cafe.Bll.Contracts.Servises.Order.Models;
using Cafe.Dal.Contracts.Repositories.Order;
using Cafe.Dal.Contracts.Repositories.Order.Models;

namespace Cafe.Bll.Infrastructure.Servises
{
    internal class OrderServise : IOrderServise
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderServise(IMapper mapper, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public List<DishDto> AddToOrder(int idOrder, int[] idDishes)
        {
            var ordersDb = _orderRepository.AddToOrder(idOrder, idDishes);
            var ordersDto = _mapper.Map<List<DishDto>>(ordersDb);
            return ordersDto;

        }

        public void CloseOrder(int idOrder)
        {
            _orderRepository.CloseOrder(idOrder);
        }

        public List<OrderDto> GetAllOpenOrders()
        {
            var ordersDb = _orderRepository.GetAllOpen();
            var ordersDto = _mapper.Map<List<OrderDto>>(ordersDb);
            return ordersDto;
        }
        public List<OrderDto> GetAllOrders()
        {
            var ordersDb = _orderRepository.GetAll();
            var ordersDto = _mapper.Map<List<OrderDto>>(ordersDb);
            return ordersDto;
        }

        public void Create(OrderDto order)
        {
            var orderDb = new OrderDb(order.CustomerId, order.TableId);
            _orderRepository.Create(orderDb);
            order.Id = orderDb.Id;
        }

        public bool IsOrderClosed(int idOrder)
        {
            var orderDB = _orderRepository.GetEntity(idOrder);

            if (orderDB.CloseDate == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string Check(int idOrder)
        {
            return _orderRepository.Check(idOrder);
        }



    }
}
