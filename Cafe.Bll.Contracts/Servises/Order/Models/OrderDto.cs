using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.OrderDish.Models;
using Cafe.Bll.Contracts.Servises.Table.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Bll.Contracts.Servises.Order.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int TableId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? CloseDate { get; set; }

        public int? Total { get; set; }

        public virtual CustomerDto Customer { get; set; } = null!;

        public virtual TableDto Table { get; set; } = null!;
        public virtual ICollection<OrdersDishDto> OrdersDishes { get; set; } = new List<OrdersDishDto>();

        [NotMapped] public List<DishDto> OrderDishes = new List<DishDto>();

        public OrderDto()
        {

        }
        public OrderDto(int customer_id, int table_id)
        {

            CustomerId = customer_id;
            TableId = table_id;

        }
    }
}
