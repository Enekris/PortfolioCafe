using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Order.Models;

namespace Cafe.Bll.Contracts.Servises.OrderDish.Models;

public class OrdersDishDto
{
    public int Id { get; set; }
    public int? OrderId { get; set; }

    public int? DishId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? DishPriceOnOrdersDate { get; set; }

    public virtual DishDto? Dish { get; set; }

    public virtual OrderDto? Order { get; set; }





    public OrdersDishDto()
    {

    }
}
