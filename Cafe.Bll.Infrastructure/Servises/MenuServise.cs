using AutoMapper;
using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Menu;
using Cafe.Dal.Contracts.Repositories.Dish;
using Cafe.Dal.Contracts.Repositories.Dish.Models;

namespace Cafe.Bll.Infrastructure.Servises
{
    internal class MenuServise : IMenuServise
    {

        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public MenuServise(IMapper mapper, IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public List<DishDto> GetAllDishes()
        {
            var dishesDb = _dishRepository.GetAll();
            var dishesDto = _mapper.Map<List<DishDto>>(dishesDb);
            return dishesDto;
        }

        public void Delete(int idDish)
        {
            _dishRepository.Delete(idDish);
        }

        public void Create(DishDto dish)
        {
            var dishDb = new DishDb(dish.ItemName, dish.Description, dish.Price);
            _dishRepository.Create(dishDb);
            dish.Id = dishDb.Id;
        }
        public bool UpdateAllParams(int idDish, string itemname, string description, int price)
        {
            DishDb dish = _dishRepository.GetEntity(idDish);
            dish.ItemName = itemname;
            dish.Description = description;
            dish.Price = price;
            dish.DataCreate = DateTime.Now;
            return _dishRepository.Update(dish);
        }
        public void UpdatePrice(int idDish, int price)
        {
            DishDb dish = _dishRepository.GetEntity(idDish);
            dish.Price = price;
            _dishRepository.Update(dish);
        }
        public void UpdateAllPricePercent(int percent)
        {
            List<DishDb> dishes = _dishRepository.GetAll();
            for (int i = 0; i < dishes.Count; i++)
            {
                dishes[i].Price = Convert.ToInt32(Math.Round(dishes[i].Price * (((double)percent / 100) + 1), 0, MidpointRounding.AwayFromZero));
                _dishRepository.Update(dishes[i]);
            }
        }

        public DishDto GetEntity(int id)
        {
            var dishDb = _dishRepository.GetEntity(id);
            var dishDto = _mapper.Map<DishDto>(dishDb);
            return dishDto;
        }

    }
}

