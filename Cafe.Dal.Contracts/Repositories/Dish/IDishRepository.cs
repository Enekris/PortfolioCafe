using Cafe.Dal.Contracts.Repositories.Dish.Models;



namespace Cafe.Dal.Contracts.Repositories.Dish
{
    public interface IDishRepository : IBaseRepository<DishDb>
    {

        public bool Update(DishDb entity);
        public bool Delete(int id);

    }
}
