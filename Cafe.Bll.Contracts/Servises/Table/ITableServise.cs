using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Bll.Contracts.Servises.Table.Models;

namespace Cafe.Bll.Contracts.Servises.Table
{
    public interface ITableServise
    {
        public List<TableDto> GetALLTables();
        public void Delete(TableDto table);
        public void Create(TableDto table);
        public bool UpdateAllParams(int id, int number, int seats);
        public bool DeteteReserved(int id);
        public bool SetReserved(int idTable, CustomerDto customer);
        public TableDto GetEntity(int id);
        public List<TableDto> GetAllReservedTables();
        public List<TableDto> GetAllNotReservedTables();








    }
}
