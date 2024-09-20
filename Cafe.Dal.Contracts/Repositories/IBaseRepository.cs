namespace Cafe.Dal.Contracts.Repositories
{
    public interface IBaseRepository<T>
    {
        bool Create(T entity);

        List<T> GetAll();
        public T GetEntity(int id);

    }
}
