using Cafe.Dal.Contracts.Repositories.OrderDish.model;

namespace Cafe.Dal.Contracts.Repositories.Dish.Models;

public class DishDb
{
    public int Id { get; set; }

    public string ItemName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }
    public DateTime DataCreate { get; set; }
    public virtual ICollection<OrdersDishDb> OrdersDishes { get; set; } = new List<OrdersDishDb>();
    public DishDb()
    {

    }
    public DishDb(string itemname, string description, int price)  //для создания 
    {
        ItemName = itemname;
        Description = description;
        Price = price;
        DataCreate = DateTime.Now;
    }

}
