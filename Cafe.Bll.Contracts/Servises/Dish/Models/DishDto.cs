using Cafe.Bll.Contracts.Servises.OrderDish.Models;

namespace Cafe.Bll.Contracts.Servises.Dish.Models
{
    public class DishDto
    {
        public int Id { get; set; }

        public string ItemName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Price { get; set; }

        public DateTime DataCreate { get; set; }
        public virtual ICollection<OrdersDishDto> OrdersDishes { get; set; } = new List<OrdersDishDto>();

        public DishDto()
        {

        }
        public DishDto(string itemname, string description, int price)  //для создания 
        {
            ItemName = itemname;
            Description = description;
            Price = price;
            DataCreate = DateTime.Now;
        }

    }
}
