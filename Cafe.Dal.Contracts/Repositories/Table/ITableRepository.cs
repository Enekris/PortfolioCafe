using Cafe.Dal.Contracts.Repositories.Table.Models;


namespace Cafe.Dal.Contracts.Repositories.Table
{
    public interface ITableRepository : IBaseRepository<TableDb>
    {
        public bool Update(TableDb entity);
        public bool Delete(int id);
        public List<TableDb> GetAllReserved();
        public List<TableDb> GetAllNotReserved();

    }
}
