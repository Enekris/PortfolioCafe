using Cafe.Bll.Contracts.Servises.Dish.Models;
namespace Cafe.Bll.Contracts.Servises.Menu
{
    public interface IMenuServise
    {
        public List<DishDto> GetAllDishes();
        public void Delete(int idDish);
        public void Create(DishDto dish);
        public bool UpdateAllParams(int idDish, string itemname, string description, int price);
        public void UpdatePrice(int idDish, int price);
        public void UpdateAllPricePercent(int percent);
        public DishDto GetEntity(int id);


    }
}

