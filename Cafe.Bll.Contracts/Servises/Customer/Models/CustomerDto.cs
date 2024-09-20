using Cafe.Bll.Contracts.Servises.Order.Models;

namespace Cafe.Bll.Contracts.Servises.Customer.Models
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? DataCreate { get; set; }


        public virtual ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public CustomerDto()
        {

        }

        public CustomerDto(int age, string firstName, string lastName, string email, string phone)  //для создания 
        {
            Age = age;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            DataCreate = DateTime.Now;
        }



    }
}
