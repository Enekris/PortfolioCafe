using AutoMapper;
using Cafe.Bll.Contracts.Servises.Customer;
using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Dal.Contracts.Repositories.Customer;
using Cafe.Dal.Contracts.Repositories.Customer.Models;


namespace Cafe.Bll.Infrastructure.Servises
{

    internal class CustomerServise : ICustomerServise
    {
        private readonly ICustomerRepositoty _customerRepository;
        private readonly IMapper _mapper;



        public CustomerServise(IMapper mapper, ICustomerRepositoty customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public List<CustomerDto> GetALLCustomers()
        {
            List<CustomerDb> customersDb = _customerRepository.GetAll();
            var customersDto = _mapper.Map<List<CustomerDto>>(customersDb);
            return customersDto;
        }

        public void Delete(CustomerDto customer)
        {
            _customerRepository.Delete(customer.Id);
        }

        public void Create(CustomerDto customer)
        {
            var customerDb = new CustomerDb(customer.Age, customer.FirstName, customer.LastName, customer.Email, customer.Phone);
            _customerRepository.Create(customerDb);
            customer.Id = customerDb.Id;
        }
        public bool UpdateAllParams(int idCustomer, int age, string firstName, string lastName, string email, string phone)
        {
            var customerDb = _customerRepository.GetEntity(idCustomer);
            customerDb.Age = age;
            customerDb.FirstName = firstName;
            customerDb.LastName = lastName;
            customerDb.Email = email;
            customerDb.Phone = phone;
            return _customerRepository.Update(customerDb);
        }

        public CustomerDto GetEntity(int id)
        {
            var customerDb = _customerRepository.GetEntity(id);
            CustomerDto customerDto = _mapper.Map<CustomerDto>(customerDb);
            return customerDto;
        }

    }
}
