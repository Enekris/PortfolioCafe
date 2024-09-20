using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Contracts.Repositories.Order.Models;


namespace Cafe.Dal.Contracts.Repositories.Order
{
    public interface IOrderRepository : IBaseRepository<OrderDb>
    {
        public List<DishDb> AddToOrder(int idOrder, int[] idDishes);
        public List<OrderDb> GetAllOpen();
        public void CloseOrder(int idOrder);
        public string Check(int id);

    }
}
