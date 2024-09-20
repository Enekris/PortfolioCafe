using Cafe.Bll.Contracts.Servises.Customer.Models;



namespace Cafe.Bll.Contracts.Servises.Customer
{
    public interface ICustomerServise
    {
        public List<CustomerDto> GetALLCustomers();
        public void Delete(CustomerDto customer);
        public void Create(CustomerDto customer);
        public bool UpdateAllParams(int idCustomer, int age, string firstName, string lastName, string email, string phone);
        public CustomerDto GetEntity(int id);

    }
}
