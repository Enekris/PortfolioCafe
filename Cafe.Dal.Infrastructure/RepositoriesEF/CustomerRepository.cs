using Cafe.Dal.Contracts.Repositories.Customer;
using Cafe.Dal.Contracts.Repositories.Customer.Models;
using Cafe.Dal.Infrastructure.DBSettingsEF;

namespace Cafe.Dal.Infrastructure.RepositoriesEF
{
    internal class CustomerRepository : ICustomerRepositoty
    {

        public bool Create(CustomerDb entity)
        {
            using CafeContext db = new CafeContext();
            var customer = new CustomerDb(entity.Age, entity.FirstName, entity.LastName, entity.Email, entity.Phone);
            db.Customers.Add(customer);
            db.SaveChanges();
            entity.Id = customer.Id;
            return true;
        }


        public bool Delete(int id)
        {
            using CafeContext db = new CafeContext();
            db.Customers.Remove(GetEntity(id));
            db.SaveChanges();
            return true;
        }

        public List<CustomerDb> GetAll()
        {
            using CafeContext db = new CafeContext();
            return db.Customers.ToList();
        }

        public CustomerDb GetEntity(int id)
        {
            using CafeContext db = new CafeContext();
            return db.Customers.Find(id);
        }


        public bool Update(CustomerDb entity)
        {
            using CafeContext db = new CafeContext();
            db.Customers.Update(entity);
            db.SaveChanges();
            return true;

        }
    }
}
