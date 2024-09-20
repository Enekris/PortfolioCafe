using Cafe.Dal.Contracts.Repositories.Customer.Models;


namespace Cafe.Dal.Contracts.Repositories.Customer
{
    public interface ICustomerRepositoty : IBaseRepository<CustomerDb>
    {
        public bool Update(CustomerDb entity);
        public bool Delete(int id);


    }
}
