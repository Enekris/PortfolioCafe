using Cafe.Dal.Contracts.Repositories.Customer.Models;
using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Contracts.Repositories.Order.Models;
using Cafe.Dal.Contracts.Repositories.OrderDish.model;
using Cafe.Dal.Contracts.Repositories.Table.Models;
using Cafe.Dal.Infrastructure.DbMaps;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Dal.Infrastructure.DBSettingsEF;

public class CafeContext : DbContext
{
    public CafeContext()
    {
    }

    public CafeContext(DbContextOptions<CafeContext> options)
        : base(options)
    {
      //  Database.EnsureCreated();
    }

    public virtual DbSet<CustomerDb> Customers { get; set; }

    public virtual DbSet<DishDb> Dishes { get; set; }

    public virtual DbSet<OrderDb> Orders { get; set; }

    public virtual DbSet<OrdersDishDb> OrdersDishes { get; set; }

    public virtual DbSet<TableDb> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Cafe;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerMap());
        modelBuilder.ApplyConfiguration(new DishMap());
        modelBuilder.ApplyConfiguration(new OrdersDishMap());
        modelBuilder.ApplyConfiguration(new OrderMap());
        modelBuilder.ApplyConfiguration(new TableMap());
    }

}
