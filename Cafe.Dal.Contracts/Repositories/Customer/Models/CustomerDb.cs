using Cafe.Dal.Contracts.Repositories.Order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Dal.Contracts.Repositories.Customer.Models;

public class CustomerDb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Age { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? DataCreate { get; set; }


    public virtual ICollection<OrderDb> Orders { get; set; } = new List<OrderDb>();

    public CustomerDb()
    {

    }

    public CustomerDb(int age, string firstName, string lastName, string email, string phone)  //для создания 
    {
        Age = age;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        DataCreate = DateTime.Now;
    }
}
