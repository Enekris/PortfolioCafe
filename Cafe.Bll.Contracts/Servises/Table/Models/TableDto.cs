using Cafe.Bll.Contracts.Servises.Order.Models;


namespace Cafe.Bll.Contracts.Servises.Table.Models
{
    public class TableDto
    {

        public int Id { get; set; }

        public int Number { get; set; }

        public int Seats { get; set; }

        public DateTime? Reserved { get; set; }
        public int? ReservedCustomerId { get; set; }
        public virtual ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public TableDto()
        {

        }

        public TableDto(int tableNumber, int seats) //для создания

        {
            Number = tableNumber;
            Seats = seats;
        }


    }
}
