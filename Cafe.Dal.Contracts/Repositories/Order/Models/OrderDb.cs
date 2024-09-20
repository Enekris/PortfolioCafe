using Cafe.Dal.Contracts.Repositories.Customer.Models;
using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Contracts.Repositories.OrderDish.model;
using Cafe.Dal.Contracts.Repositories.Table.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Dal.Contracts.Repositories.Order.Models;

public class OrderDb
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int TableId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public DateTime? CloseDate { get; set; }

    public int? Total { get; set; }

    public virtual CustomerDb Customer { get; set; } = null!;
    public virtual TableDb Table { get; set; } = null!;

    public virtual ICollection<OrdersDishDb> OrdersDishes { get; set; } = new List<OrdersDishDb>();

    [NotMapped] public List<DishDb> OrderDishes = new List<DishDb>();

    public OrderDb()
    {

    }
    public OrderDb(int customer_id, int table_id)  //для создания 
    {

        CustomerId = customer_id;
        TableId = table_id;


    }
}
