using Cafe.Dal.Contracts.Repositories.Order.Models;

namespace Cafe.Dal.Contracts.Repositories.Table.Models;

public class TableDb
{
    public int Id { get; set; }

    public int Number { get; set; }

    public int Seats { get; set; }

    public DateTime? Reserved { get; set; }

    public int? ReservedCustomerId { get; set; }

    public virtual ICollection<OrderDb> Orders { get; set; } = new List<OrderDb>();

    public TableDb()
    {

    }

    public TableDb(int tableNumber, int seats) //для создания

    {
        Number = tableNumber;
        Seats = seats;
    }
}
