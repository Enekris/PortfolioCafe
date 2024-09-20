using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Order.Models;

namespace Cafe.Bll.Contracts.Servises.Order
{
    public interface IOrderServise
    {
        public List<DishDto> AddToOrder(int idOrder, int[] idDishes);
        public void CloseOrder(int idOrder);
        public List<OrderDto> GetAllOpenOrders();
        public List<OrderDto> GetAllOrders();
        public void Create(OrderDto order);
        public bool IsOrderClosed(int idOrder);
        public string Check(int idOrder);

    }
}
