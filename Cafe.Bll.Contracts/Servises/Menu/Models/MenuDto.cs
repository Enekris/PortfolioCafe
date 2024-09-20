using Cafe.Bll.Contracts.Servises.Dish.Models;


namespace Cafe.Bll.Contracts.Servises.Menu.Models
{
    public class MenuDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<DishDto> Dishes = new List<DishDto>();


        public MenuDto()
        {

        }
        public MenuDto(string name, int id, List<DishDto> dishes)
        {
            Name = name;
            Id = id;
            Dishes = dishes;
        }



    }
}
