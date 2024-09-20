using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Contracts.Repositories.Order.Models;

namespace Cafe.Dal.Contracts.Repositories.OrderDish.model;

public class OrdersDishDb
{
    public int Id { get; set; }
    public int? OrderId { get; set; }

    public int? DishId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? DishPriceOnOrdersDate { get; set; }

    public virtual DishDb? Dish { get; set; }

    public virtual OrderDb? Order { get; set; }





    public OrdersDishDb()
    {

    }
}
